namespace Common.MessageBroker.Saga.Common.Messages;
public record UpdateAchievementsMessage(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);