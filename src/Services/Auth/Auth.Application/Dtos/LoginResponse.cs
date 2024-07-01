namespace Auth.Application.Dtos;

public record LoginResponse(
    string AccessToken,
    AthleteResponse Athlete);

public record AthleteResponse(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    DateTime CreatedAt,
    string ProfileMedium,
    string Profile);