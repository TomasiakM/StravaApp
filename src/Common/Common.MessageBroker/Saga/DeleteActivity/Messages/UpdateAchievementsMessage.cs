namespace Common.MessageBroker.Saga.DeleteActivity.Messages;
public record UpdateAchievementsMessage(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);