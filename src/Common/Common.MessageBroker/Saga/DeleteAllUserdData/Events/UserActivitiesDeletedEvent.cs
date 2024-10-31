namespace Common.MessageBroker.Saga.DeleteAllUserdData.Events;
public record UserActivitiesDeletedEvent(
    Guid CorrelationId,
    long StravaUserId);
