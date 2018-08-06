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



[<RequireQualifiedAccess>]
module ProducerResponse =
  let empty() = ProducerResponse()

  let (|IsProducerConnected|IsProducerQuotaAssigned|IsProducerAccepted|IsProducerRejected|IsEmptyProducerResponse|)
    (response: ProducerResponse) =
    match response.PayloadCase with
    | ProducerResponse.PayloadOneofCase.Connected ->
      IsProducerConnected response.Connected
    | ProducerResponse.PayloadOneofCase.QuotaAssigned ->
      IsProducerQuotaAssigned response.QuotaAssigned
    | ProducerResponse.PayloadOneofCase.Accepted ->
      IsProducerAccepted response.Accepted
    | ProducerResponse.PayloadOneofCase.Rejected ->
      IsProducerRejected response.Rejected
    | _ ->
      IsEmptyProducerResponse

  [<RequireQualifiedAccess>]
  module RequestId =
    let get (m: ProducerRequest) = m.RequestId
    let set key (m: ProducerRequest) =
      do m.RequestId <- emptyStringIfNull key
      m
    let has m = get m |> isEmptyString |> not
    let lens = get, set

[<RequireQualifiedAccess>]
module ProducerConnected =
  let empty() = ProducerConnected()

  [<RequireQualifiedAccess>]
  module Quota =
    let get (m: ProducerConnected) = m.Quota
    let set key (m: ProducerConnected) =
      do m.Quota <- key
      m
    let lens = get, set

[<RequireQualifiedAccess>]
module ProducerQuotaAssignment =
  let empty() = ProducerQuotaAssignment()

  [<RequireQualifiedAccess>]
  module Quota =
    let get (m: ProducerQuotaAssignment) = m.Quota
    let set key (m: ProducerQuotaAssignment) =
      do m.Quota <- key
      m
    let lens = get, set

[<RequireQualifiedAccess>]
module ProducerAccepted =
  let empty() = ProducerAccepted()

[<RequireQualifiedAccess>]
module ProducerRejected =
  let empty() = ProducerRejected()

  [<RequireQualifiedAccess>]
  module ErrorCode =
    let get (m: ProducerRejected) = m.ErrorCode
    let set key (m: ProducerRejected) =
      do m.ErrorCode <- key
      m
    let lens = get, set

  [<RequireQualifiedAccess>]
  module ErrorMessage =
    let get (m: ProducerRejected) = m.ErrorMessage
    let set key (m: ProducerRejected) =
      do m.ErrorMessage <- key
      m
    let lens = get, set
