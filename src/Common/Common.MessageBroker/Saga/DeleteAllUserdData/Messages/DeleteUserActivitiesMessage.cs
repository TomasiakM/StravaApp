namespace Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
public record DeleteUserActivitiesMessage(
    Guid CorrelationId,
    long StravaUserId);

