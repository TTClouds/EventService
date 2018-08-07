[<AutoOpen>]
module EventService.Abstractions.Common

open System

type ErrorCodes =
  | None = 0
  | Unknown = 1
  | InvalidToken = 2
  | InvalidTopic = 3
  | InvalidSubscription = 4
  | QuotaExceeded = 5
  | Closing = 6


type EventSubject = {
  domain: string
  entity: string
  id: string
}

type EventMessage = {
  mimeType: string
  target: EventSubject
  source: EventSubject option
  correlationId: string
  eventType: string
  eventTime: DateTimeOffset
  targetSequence: int32 option
  topicSequence: int32 option
  receiveTime: DateTimeOffset option
  processTime: DateTimeOffset option
  meta: Map<string, string>
  data: byte[]
}

type AssignedQuota = {
  messagesCount: int64
  bytesCount: int64
}

type StringEquality = StringEquality of string

type TopicQuery =
  | ConjunctiveTopicQuery of ConjunctiveTopicQuery

and ConjunctiveTopicQuery = {
  targetDomain: StringEquality option
  targetEntity: StringEquality option
  targetId: StringEquality option
  correlationId: StringEquality option
  eventType: StringEquality option
  meta: Map<string, StringEquality> option
}
