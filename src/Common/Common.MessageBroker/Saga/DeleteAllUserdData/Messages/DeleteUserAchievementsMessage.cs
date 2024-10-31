namespace Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
public record DeleteUserAchievementsMessage(
    Guid CorrelationId,
    long StravaUserId);
