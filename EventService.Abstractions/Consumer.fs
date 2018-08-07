module EventService.Abstractions.Consumer

type ConsumerRequest = {
  requestId: string;
  payload: ConsumerRequestPayload
}

and ConsumerRequestPayload =
  | ConnectConsumer of ConnectConsumer
  | CloseConsumer

and ConnectConsumer = {
  authToken: string
  topic: string
  subscription: string
}

type ConsumerResponse = {
  ResponseId: string;
  payload: ConsumerResponsePayload
}

and ConsumerResponsePayload =
  | ConsumerConnected
  | ConsumerNext of ConsumerNext
  | ConsumerError of ConsumerError
  | ConsumerDone
  | ConsumerRejected of ConsumerRejected

and ConsumerNext = {
  results: EventMessage list
}

and ConsumerError = {
  errorCode: ErrorCodes
  errorMessage: string
}

and ConsumerRejected = {
  errorCode: ErrorCodes
  errorMessage: string
}

type ConsumerService = StreamingService<ConsumerRequest, ConsumerResponse>
