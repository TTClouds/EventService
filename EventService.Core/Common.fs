namespace EventService.Protocol
open Google.Protobuf

[<AutoOpen>]
module internal Utils =
  let isEmptyString s = System.String.IsNullOrEmpty s
  let emptyStringIfNull s = if isEmptyString s then "" else s
  let isEmptyBytes (s: ByteString) = s.IsEmpty
  let emptyBytesIfNull s = if isEmptyBytes s then ByteString.Empty else s

[<RequireQualifiedAccess>]
module EventMessage =
  let empty() = EventMessage()

  [<RequireQualifiedAccess>]
  module Key =
    let get (m: EventMessage) = m.Key
    let set key (m: EventMessage) =
      do m.Key <- emptyStringIfNull key
      m
    let has m = get m |> isEmptyString |> not
    let lens = get, set

  [<RequireQualifiedAccess>]
  module Meta =
    let get (m: EventMessage) = m.Meta
    let add key value m =
      let map = get m
      do map.Add(key, value)
      m
    let remove key m =
      let map = get m
      do map.Remove(key) |> ignore
      m
    let tryGet key m =
      let map = get m
      let found, value = map.TryGetValue(key)
      if found then Some value else None

  [<RequireQualifiedAccess>]
  module Data =
    let get (m: EventMessage) = m.Data
    let set data (m: EventMessage) =
      do m.Data <- emptyBytesIfNull data
      m
    let has m = get m |> isEmptyBytes |> not
    let lens = get, set

    let getArray m = get m |> fun b -> b.ToByteArray()
    let setArray arr = ByteString.CopyFrom arr |> set
    let arrayLens = getArray, setArray

    let getBase64 m = get m |> fun b -> b.ToBase64()
    let setBase64 arr = ByteString.FromBase64 arr |> set
    let base64Lens = getBase64, setBase64

[<RequireQualifiedAccess>]
module AssignedQuota =
  let empty() = AssignedQuota()

