namespace Common.MessageBroker.Saga.DeleteAllUserdData.Events;
public record UserTilesDeletedEvent(
    Guid CorrelationId,
    long StravaUserId);
