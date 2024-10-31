namespace Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
public record DeleteUserTilesMessage(
    Guid CorrelationId,
    long StravaUserId);

