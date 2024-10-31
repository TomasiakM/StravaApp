namespace Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
public record DeleteUserTokenMessage(
    Guid CorrelationId,
    long StravaUserId);
