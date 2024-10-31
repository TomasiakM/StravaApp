namespace Common.MessageBroker.Saga.DeleteAllUserdData.Events;
public record UserTokenDeletedEvent(
    Guid CorrelationId,
    long StravaUserId);
