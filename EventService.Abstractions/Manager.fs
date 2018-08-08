module EventService.Abstractions.Manager

type ManagerRequest = {
  requestId: string;
  payload: ManagerRequestPayload
}

and ManagerRequestPayload =
  | ConnectManager of ConnectManager
  | ListTopicsManager
  | DetailsTopicManager of DetailsTopicManager
  | CreateTopicManager of CreateTopicManager
  | RemoveTopicManager of RemoveTopicManager
  | ListSubscriptionsManager of ListSubscriptionsManager
  | DetailsSubscriptionManager of DetailsSubscriptionManager
  | CloseManager

and ConnectManager = {
  authToken: string
}

and DetailsTopicManager = {
  topic: string
}

and CreateTopicManager = {
  topic: string
}

and RemoveTopicManager = {
  topic: string
}

and ListSubscriptionsManager = {
  topic: string
}

and DetailsSubscriptionManager = {
  topic: string
  subscription: string
}

type ManagerResponse = {
  ResponseId: string;
  payload: ManagerResponsePayload
}

and ManagerResponsePayload =
  | ManagerConnected
  | ManagerQueryNext of ManagerQueryNext
  | ManagerQueryError of ManagerQueryError
  | ManagerQueryDone of ManagerQueryDone
  | ManagerRejected of ManagerRejected

and ManagerQueryNext = {
  queryId: string
  results: EventMessage list
}

and ManagerQueryError = {
  queryId: string
  errorCode: ErrorCodes
  errorMessage: string
}

and ManagerQueryDone = {
  queryId: string
}

and ManagerRejected = {
  errorCode: ErrorCodes
  errorMessage: string
}

type ManagerService = StreamingService<ManagerRequest, ManagerResponse>
