namespace Common.MessageBroker.Saga.DeleteActivity.Messages;
public record DeleteActivityMessage(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);