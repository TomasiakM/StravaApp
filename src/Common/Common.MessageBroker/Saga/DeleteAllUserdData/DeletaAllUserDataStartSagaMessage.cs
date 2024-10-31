namespace Common.MessageBroker.Saga.DeleteAllUserdData;
public record DeletaAllUserDataStartSagaMessage(
    Guid CorrelationId,
    long StravaUserId);
