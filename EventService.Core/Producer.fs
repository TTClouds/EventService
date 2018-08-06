namespace EventService.Protocol
open Google.Protobuf

[<RequireQualifiedAccess>]
module ConnectProducer =
  let empty() = ConnectProducer()

  [<RequireQualifiedAccess>]
  module AuthToken =
    let get (m: ConnectProducer) = m.AuthToken
    let set key (m: ConnectProducer) =
      do m.AuthToken <- emptyStringIfNull key
      m
    let has m = get m |> isEmptyString |> not
    let lens = get, set

  [<RequireQualifiedAccess>]
  module Topic =
    let get (m: ConnectProducer) = m.Topic
    let set key (m: ConnectProducer) =
      do m.Topic <- emptyStringIfNull key
      m
    let has m = get m |> isEmptyString |> not
    let lens = get, set

[<RequireQualifiedAccess>]
module SendProducer =
  let empty() = SendProducer()

  [<RequireQualifiedAccess>]
  module Events =
    let get (m: SendProducer) = m.Events
    let count m = get m |> fun e -> e.Count
    let clean m =
      do get m |> fun e -> e.Clear()
      m
    let add ev m =
      do get m |> fun e -> e.Add(ev: EventMessage)
      m
    let addMany evs m =
      do get m |> fun e -> e.Add(evs: EventMessage seq)
      m

[<RequireQualifiedAccess>]
module CloseProducer =
  let empty() = CloseProducer()

[<RequireQualifiedAccess>]
module ProducerRequest =
  let empty() = ProducerRequest()

  let (|IsConnectProducer|IsSendProducer|IsCloseProducer|IsEmptyProducerRequest|)
    (request: ProducerRequest) =
    match request.PayloadCase with
    | ProducerRequest.PayloadOneofCase.Connect ->
      IsConnectProducer request.Connect
    | ProducerRequest.PayloadOneofCase.Send ->
      IsSendProducer request.Send
    | ProducerRequest.PayloadOneofCase.Close ->
      IsCloseProducer request.Close
    | _ ->
      IsEmptyProducerRequest

  [<RequireQualifiedAccess>]
  module RequestId =
    let get (m: ProducerRequest) = m.RequestId
    let set key (m: ProducerRequest) =
      do m.RequestId <- emptyStringIfNull key
      m
    let has m = get m |> isEmptyString |> not
    let lens = get, set


