namespace EventService.Abstractions
open EventService.Abstractions.Producer
open EventService.Abstractions.Reader

type EventServiceStruct = {
  producer: ProducerService
  reader: ReaderService
}

// type IEventService =
//   inherit IProducerService
//   inherit IReaderService

// module EventService =
//   let asClass st = {
//     new IEventService with
//       member __.StartProducer rs = st.producer rs
//       member __.StartReader rs = st.reader rs
//   }
