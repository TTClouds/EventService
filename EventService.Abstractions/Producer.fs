module EventService.Abstractions.Producer

type ProducerRequest = {
  requestId: string;
  payload: ProducerRequestPayload
}

and ProducerRequestPayload =
  | ConnectProducer of ConnectProducer
  | SendProducer of SendProducer
  | CloseProducer

and ConnectProducer = {
  authToken: string
  topic: string
}

and SendProducer = {
  events: EventMessage list
}

type ProducerResponse = {
  requestId: string;
  payload: ProducerResponsePayload
}

and ProducerResponsePayload =
  | ProducerConnected of ProducerConnected
  | ProducerQuotaAssignment of ProducerQuotaAssignment
  | ProducerAccepted
  | ProducerRejected of ProducerRejected

and ProducerConnected = {
  quota: AssignedQuota
}

and ProducerQuotaAssignment = {
  quota: AssignedQuota
}

and ProducerRejected = {
  errorCode: ErrorCodes
  errorMessage: string
}

type ProducerService = StreamingService<ProducerRequest, ProducerResponse>
