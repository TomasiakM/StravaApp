namespace Common.MessageBroker.Saga.DeleteAllUserdData;
public record DeleteAllUserDataStartSagaMessage(
    Guid CorrelationId,
    long StravaUserId);
