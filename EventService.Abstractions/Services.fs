namespace EventService.Abstractions
open EventService.Abstractions.Producer
open EventService.Abstractions.Reader
open EventService.Abstractions.Consumer
open System

type ProducerService = IObservable<ProducerRequest> -> IObservable<ProducerResponse>

type IProducerService =
  abstract member StartProducer:
    IObservable<ProducerRequest> -> IObservable<ProducerResponse>

type ReaderService = IObservable<ReaderRequest> -> IObservable<ReaderResponse>

type IReaderService =
  abstract member StartReader:
    IObservable<ReaderRequest> -> IObservable<ReaderResponse>

type ConsumerService = IObservable<ConsumerRequest> -> IObservable<ConsumerResponse>

type IConsumerService =
  abstract member StartConsumer:
    IObservable<ConsumerRequest> -> IObservable<ConsumerResponse>

type EventServiceStruct = {
  producer: ProducerService
  reader: ReaderService
}

type IEventService =
  inherit IProducerService
  inherit IReaderService

module EventService =
  let asClass st = {
    new IEventService with
      member __.StartProducer rs = st.producer rs
      member __.StartReader rs = st.reader rs
  }
