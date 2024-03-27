namespace Strava.Application.Dtos.Athlete;
public record AthleteSummitResponse(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    DateTime CreatedAt,
    string ProfileMedium,
    string Profile);
