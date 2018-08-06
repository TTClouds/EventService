namespace EventService.Protocol

open NUnit.Framework
open FsUnit
open Google.Protobuf
open Google.Protobuf

module EventMessageTests =

  [<Test>]
  let ``EventMessage.empty``() =
    let m = EventMessage.empty()
    do m |> should not' (be Null)
    do m.Key |> should be EmptyString
    do m.Meta |> should not' (be Null)
    do m.Data |> should not' (be Null)

  [<Test>]
  let ``EventMessage.Key``() =
    let m = EventMessage.empty()
            |> EventMessage.Key.set "1234"
    do m |> EventMessage.Key.get |> should equal "1234"

  [<Test>]
  let ``EventMessage.Meta``() =
    let m = EventMessage.empty()
            |> EventMessage.Meta.add "1" "One"
            |> EventMessage.Meta.add "2" "Two"
            |> EventMessage.Meta.add "3" "Three"
            |> EventMessage.Meta.remove "3"
    do m |> EventMessage.Meta.tryGet "A" |> should equal None
    do m |> EventMessage.Meta.tryGet "1" |> should equal (Some "One")
    do m |> EventMessage.Meta.tryGet "2" |> should equal (Some "Two")
    do m |> EventMessage.Meta.tryGet "3" |> should equal None

  [<Test>]
  let ``EventMessage.Data``() =
    let arr = [| 1uy; 2uy; 3uy |]
    let m = EventMessage.empty()
            |> EventMessage.Data.set (ByteString.CopyFrom(arr))
    do m |> EventMessage.Data.get
         |> (fun b -> b.ToByteArray())
         |> should equal arr

  [<Test>]
  let ``EventMessage.Data.array``() =
    let arr = [| 1uy; 2uy; 3uy |]
    let m = EventMessage.empty()
            |> EventMessage.Data.setArray arr
    do m |> EventMessage.Data.getArray
         |> should equal arr

  [<Test>]
  let ``EventMessage.Data.base64``() =
    let arr = [| 1uy; 2uy; 3uy |] |> System.Convert.ToBase64String
    let m = EventMessage.empty()
            |> EventMessage.Data.setBase64 arr
    do m |> EventMessage.Data.getBase64
         |> should equal arr

  [<Test>]
  let ``EventMessage.Data.utf8``() =
    let arr = "Hello world"
    let m = EventMessage.empty()
            |> EventMessage.Data.setUTF8 arr
    do m |> EventMessage.Data.getUTF8
         |> should equal arr

