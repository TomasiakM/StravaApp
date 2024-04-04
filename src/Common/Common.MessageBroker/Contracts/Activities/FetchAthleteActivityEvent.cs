namespace Common.MessageBroker.Contracts.Activities;
public record FetchAthleteActivityEvent(
    long StravaUserId,
    long StravaActivityId);
