module EventService.Abstractions.Reader

type ReaderRequest = {
  requestId: string;
  payload: ReaderRequestPayload
}

and ReaderRequestPayload =
  | ConnectReader of ConnectReader
  | StartQueryReader of StartQueryReader
  | ContinueQueryReader of ContinueQueryReader
  | FinishQueryReader of FinishQueryReader
  | CloseReader

and ConnectReader = {
  authToken: string
  topic: string
}

and StartQueryReader = {
  queryId: string
  quota: AssignedQuota
  query: TopicQuery
}

and ContinueQueryReader = {
  queryId: string
  quota: AssignedQuota
}

and FinishQueryReader = {
  queryId: string
}

type ReaderResponse = {
  ResponseId: string;
  payload: ReaderResponsePayload
}

and ReaderResponsePayload =
  | ReaderConnected
  | ReaderQueryNext of ReaderQueryNext
  | ReaderQueryError of ReaderQueryError
  | ReaderQueryDone of ReaderQueryDone
  | ReaderRejected of ReaderRejected

and ReaderQueryNext = {
  queryId: string
  results: EventMessage list
}

and ReaderQueryError = {
  queryId: string
  errorCode: ErrorCodes
  errorMessage: string
}

and ReaderQueryDone = {
  queryId: string
}

and ReaderRejected = {
  errorCode: ErrorCodes
  errorMessage: string
}

