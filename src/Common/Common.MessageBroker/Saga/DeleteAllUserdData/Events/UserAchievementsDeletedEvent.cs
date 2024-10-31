namespace Common.MessageBroker.Saga.DeleteAllUserdData.Events;
public record UserAchievementsDeletedEvent(
    Guid CorrelationId,
    long StravaUserId);
