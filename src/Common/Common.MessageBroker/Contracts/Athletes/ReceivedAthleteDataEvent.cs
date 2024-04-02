namespace Common.MessageBroker.Contracts.Athletes;
public record ReceivedAthleteDataEvent(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    DateTime CreatedAt,
    string ProfileMedium,
    string Profile);
