namespace Common.MessageBroker.Saga.ProcessActivityData.Events;
public record TilesProcessedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);
