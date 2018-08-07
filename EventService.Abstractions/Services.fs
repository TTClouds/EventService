namespace EventService.Abstractions
open EventService.Abstractions.Producer
open System

type IProducerService =
  abstract member StartProducer:
    IObservable<ProducerRequest> -> IObservable<ProducerResponse>