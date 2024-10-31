namespace Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
public record DeleteUserDetailsMessage(
    Guid CorrelationId,
    long StravaUserId);

