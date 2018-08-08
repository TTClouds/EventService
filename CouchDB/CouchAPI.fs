namespace CouchDB.CouchAPI

open System.Net.Http
open System
open System.Net.Http.Headers
open Newtonsoft.Json
open System.Text
open System.IO

[<AutoOpen>]
module Basics =
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
      | ex -> invalidOp (sprintf "Invalid charset: '%s'"  encStr)
    let! stream = Async.AwaitTask <| response.Content.ReadAsStreamAsync()
    let reader = new StreamReader(stream, encoding)
    return reader
  }

  let fromJson<'a> (response: HttpResponseMessage) = async {
    let! string = Async.AwaitTask <| response.Content.ReadAsStringAsync()
    let serializer = JsonSerializer()
    serializer.Deserialize(reader)
    return ()
  }

  let getAsync (client: HttpClient) = ()

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

  let getInfo (client: HttpClient) = async {
    use request = new HttpRequestMessage(HttpMethod.Get, Uri("/", UriKind.Relative))
    do request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue("application/json"))
    let! response = Async.AwaitTask <| client.SendAsync(request)
    if response.IsSuccessStatusCode then
      () // let! string = Async.AwaitTask <| response.Content.ReadAsStringAsync()

    else
      ()

    return ()
  }