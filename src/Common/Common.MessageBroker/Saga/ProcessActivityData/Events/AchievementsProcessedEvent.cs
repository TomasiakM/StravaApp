namespace Common.MessageBroker.Saga.ProcessActivityData.Events;
public record AchievementsProcessedEvent(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);
