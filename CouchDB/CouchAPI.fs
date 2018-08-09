namespace CouchDB.CouchAPI

open System.Net.Http
open System
open System.Net.Http.Headers
open Newtonsoft.Json
open System.Text
open System.IO
open Newtonsoft.Json.Linq
open Newtonsoft.Json
open System.Net.Http
open System.Text.RegularExpressions
open System.Text.RegularExpressions
open System.Text.RegularExpressions

[<AutoOpen>]
module Basics =
  type ErrorReason = {
    error: string
    reason: string
  }

  type NamePassword = {
    name: string
    password: string
  }

  let getString (response: HttpResponseMessage) =
    Async.AwaitTask <| response.Content.ReadAsStringAsync()

  let getStream (response: HttpResponseMessage) =
    Async.AwaitTask <| response.Content.ReadAsStreamAsync()

  let getTextReader (response: HttpResponseMessage) = async {
    let encoding =
      let encStr =
        let ct = response.Content.Headers.ContentType
        if ct = null || ct.CharSet = null then
          Encoding.UTF8.WebName
        else
          ct.CharSet
      try
        Encoding.GetEncoding encStr
      with
      | _ -> invalidOp (sprintf "Invalid charset: '%s'"  encStr)
    let! stream = getStream response
    let reader = new StreamReader(stream, encoding)
    return reader
  }

  let fromJson<'a> (response: HttpResponseMessage) = async {
    let! text = getString response
    if response.Content.Headers.ContentType.MediaType = "application/json" then
      try
        return JsonConvert.DeserializeObject<'a>(text) |> Ok
      with ex ->
        return Error [ sprintf "%s: %s" (ex.GetType().Name) ex.Message ]
    else
      return Error [sprintf "Unexpected ContentType: %O" response.Content.Headers.ContentType]
  }

  let send<'a> (client: HttpClient) request =
    Async.AwaitTask <| client.SendAsync(request)

  let extractJson<'a, 'b> (response: HttpResponseMessage) = async {
    if response.IsSuccessStatusCode then
      let! j = fromJson<'a> response
      match j with
      | Ok a -> return Ok a
      | Error es -> return Error (None, response.StatusCode, es)
    else
      let! j = fromJson<'b> response
      match j with
      | Ok b -> return Error (Some b, response.StatusCode, [ response.ReasonPhrase ])
      | Error es -> return Error (None, response.StatusCode, response.ReasonPhrase :: es)
  }

  let sendForJson<'a, 'b> (client: HttpClient) request = async {
    let! response = send client request
    return! extractJson<'a, 'b> response
  }

module Server =
  type ServerInfo = {
    couchdb: string
    uuid: string
    vendor: ServerInfoVendor
    version: string
  }
  and ServerInfoVendor = {
    name: string
    version: string
  }

  type DbUpdatesResults = {
    results: DbUpdatesResultItem list
    [<JsonProperty("last_seq")>]
    lastSeq: string
  }
  and DbUpdatesResultItem = {
    [<JsonProperty("db_name")>]
    dbName: string
    [<JsonProperty("type")>]
    updateType: string
    [<JsonProperty("seq")>]
    sequence: string
  }

  type UUIDsResult = {
    uuids: string list
  }

  type PostSessionResult = {
    ok: bool
    name: string
    roles: string list
    cookie: string
  }

  let makeGet uri =
    new HttpRequestMessage(HttpMethod.Get, Uri(uri, UriKind.Relative))

  let makePost uri content =
    let req = new HttpRequestMessage(HttpMethod.Post, Uri(uri, UriKind.Relative))
    req.Content <- content
    req

  let makePostJson uri data =
    let json = JsonConvert.SerializeObject(data)
    let content = new StringContent(json, Encoding.UTF8, "application/json")
    makePost uri content

  let getInfo client = async {
    use request = makeGet "/"
    return! sendForJson<ServerInfo, ErrorReason> client request
  }

  let getAllDbs client = async {
    use request = makeGet "/_all_dbs"
    return! sendForJson<string list, ErrorReason> client request
  }

  let getUUIDs client count = async {
    let uri =
      if count = 1 then "/_uuids"
      else sprintf "/_uuids?count=%i" count
    use request = makeGet uri
    return! sendForJson<UUIDsResult, ErrorReason> client request
  }

  let private authSessionCookieExpr = new Regex(@"AuthSession=(?<cookie>[^;]+)(;|$)", RegexOptions.Compiled ||| RegexOptions.IgnoreCase)
  let postSession client name password = async {
    let data = { name = name; password = password }
    use request = makePostJson "/_session" data
    let! response = send client request
    let! result = extractJson<PostSessionResult, ErrorReason> response
    match result with
    | Ok result ->
      let found, values = response.Headers.TryGetValues "Set-Cookie"
      let cookie =
        if found then
          values
            |> Seq.map (fun v -> authSessionCookieExpr.Match v)
            |> Seq.map (fun m -> if m.Success then Some m.Groups.["cookie"].Value else None)
            |> Seq.filter Option.isSome
            |> Seq.tryHead
            |> Option.flatten
        else None
      match cookie with
      | Some cookie ->
        return Ok { result with cookie = cookie }
      | None ->
        let err = { error = "Bad Response"; reason = "Expected header Set-Cookie not found" }
        return Error (Some err, response.StatusCode, [response.ReasonPhrase])
    | _ -> return result
  }
