namespace Common.MessageBroker.Saga.Common.Events;
public record AchievementsUpdatedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);