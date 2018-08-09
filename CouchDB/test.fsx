// #r "../packages/NETStandard.Library.NETFramework/build/net462/lib/System.Runtime.InteropServices.RuntimeInformation.dll"
#r "../packages/FSharp.Core/lib/net45/FSharp.Core.dll"
// #r "../packages/System.Net.Http/lib/net46/System.Net.Http.dll"
#r "bin/Debug/netstandard2.0/CouchDB.dll"
#r "System.Net.Http"

open System
open CouchDB.CouchAPI
open System.Net.Http
open System.Net.Http.Headers
open System.Text

let createLocalClient() =
  let handler = new HttpClientHandler()
  let client = new HttpClient(handler, true)
  client.BaseAddress <- Uri("http://localhost:5984/", UriKind.Absolute)
  client

let createLocalAuthClient username password =
  let client = createLocalClient()
  let up = sprintf "%s:%s" username password |> Encoding.UTF8.GetBytes |> Convert.ToBase64String
  client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Basic", up)
  client

let testGetInfo() = async {
  printfn "TEST Server.getInfo"
  use client = createLocalClient()
  let! result = Server.getInfo client
  printfn "%A" result
  return ()
}

let testAllDbs() = async {
  printfn "TEST Server.getAllDbs"
  use client = createLocalClient()
  let! result = Server.getAllDbs client
  printfn "%A" result
  return ()
}

let testUUIDs() = async {
  printfn "TEST Server.getUUIDs"
  use client = createLocalAuthClient "Admin" "Admin"
  let! result = Server.getUUIDs client 10
  printfn "%A" result
  return ()
}

let testpostSession() = async {
  printfn "TEST Server.postSession"
  use client = createLocalClient ()
  let! result = Server.postSession client "Admin" "Admin"
  printfn "%A" result
  return ()
}

// let getInfo = Server.getInfo
printfn "Is working"
Async.RunSynchronously <| testpostSession()
printfn "Done!"
