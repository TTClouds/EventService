module EventService.Abstractions.Consumer

type ConsumerRequest = {
  requestId: string;
  payload: ConsumerRequestPayload
}

and ConsumerRequestPayload =
  | ConnectConsumer of ConnectConsumer
  | StartQueryConsumer of StartQueryConsumer
  | ContinueQueryConsumer of ContinueQueryConsumer
  | FinishQueryConsumer of FinishQueryConsumer
  | CloseConsumer

and ConnectConsumer = {
  authToken: string
  topic: string
}

and StartQueryConsumer = {
  queryId: string
  quota: AssignedQuota
  query: TopicQuery
}

and ContinueQueryConsumer = {
  queryId: string
  quota: AssignedQuota
}

and FinishQueryConsumer = {
  queryId: string
}

type ConsumerResponse = {
  ResponseId: string;
  payload: ConsumerResponsePayload
}

and ConsumerResponsePayload =
  | ConsumerConnected
  | ConsumerQueryNext of ConsumerQueryNext
  | ConsumerQueryError of ConsumerQueryError
  | ConsumerQueryDone of ConsumerQueryDone
  | ConsumerRejected of ConsumerRejected

and ConsumerQueryNext = {
  queryId: string
  results: EventMessage list
}

and ConsumerQueryError = {
  queryId: string
  errorCode: ErrorCodes
  errorMessage: string
}

and ConsumerQueryDone = {
  queryId: string
}

and ConsumerRejected = {
  errorCode: ErrorCodes
  errorMessage: string
}

