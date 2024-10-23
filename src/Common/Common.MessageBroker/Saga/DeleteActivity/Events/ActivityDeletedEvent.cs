namespace Common.MessageBroker.Saga.DeleteActivity.Events;
public record ActivityDeletedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);
