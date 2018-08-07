module EventService.InMemory

open FSharp.Core
open EventService.Protocol
open EventService.Abstractions
open EventService.Abstractions.Producer
open EventService.Abstractions.Reader
open EventService.Abstractions.Consumer
open Hopac
open Hopac.Infixes

// type TopicImpl = {
//   startProducer: ProducerService
//   startReader: ReaderService
// }

let createTopic name = job {
  let events = ref []

  return ()
}

let create(): EventServiceStruct =


  let producer callback =
    let sink _ = ()
    sink

  let reader callback =
    let sink _ = ()
    sink

  { producer = producer
    reader = reader }
