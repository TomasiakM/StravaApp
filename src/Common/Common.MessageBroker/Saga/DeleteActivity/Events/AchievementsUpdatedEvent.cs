namespace Common.MessageBroker.Saga.DeleteActivity.Events;
public record AchievementsUpdatedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);