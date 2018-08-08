namespace CouchDB.CouchAPI

open System.Net.Http
open System
open System.Net.Http.Headers

[<AutoOpen>]
module Basics =
  let fromJson<'a> (response: HttpResponseMessage) = async {
    let! string = Async.AwaitTask <| response.Content.ReadAsStringAsync()
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