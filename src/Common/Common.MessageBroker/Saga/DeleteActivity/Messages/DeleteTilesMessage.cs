namespace Common.MessageBroker.Saga.DeleteActivity.Messages;
public record DeleteTilesMessage(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);