namespace Strava.Contracts.Athlete;
public record StravaAthleteSummaryResponse(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    DateTime CreatedAt,
    string ProfileMedium,
    string Profile);
