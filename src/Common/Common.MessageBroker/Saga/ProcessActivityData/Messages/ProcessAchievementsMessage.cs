namespace Common.MessageBroker.Saga.ProcessActivityData.Messages;
public record ProcessAchievementsMessage(
    Guid CorrelationId,
    long StravaActivityId,
    long StravaUserId);