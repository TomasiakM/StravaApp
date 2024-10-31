namespace Common.MessageBroker.Saga.DeleteAllUserdData.Events;
public record UserDetailsDeletedEvent(
    Guid CorrelationId,
    long StravaUserId);
