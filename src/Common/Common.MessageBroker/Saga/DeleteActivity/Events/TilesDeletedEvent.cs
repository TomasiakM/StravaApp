namespace Common.MessageBroker.Saga.DeleteActivity.Events;
public record TilesDeletedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);
