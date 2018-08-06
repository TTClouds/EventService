namespace EventService.Protocol

open NUnit.Framework
open FsUnit

module EventMessageTests =

  [<Test>]
  let ``EventMessage.empty``() =
    let m = EventMessage.empty
    m |> should not' (be Null)
