namespace Common.MessageBroker.Saga.DeleteActivity;
public record DeleteActivityDataMessage(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);
