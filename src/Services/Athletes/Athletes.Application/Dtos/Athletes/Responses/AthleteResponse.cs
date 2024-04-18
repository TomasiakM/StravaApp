namespace Athletes.Application.Dtos.Athletes.Responses;
public record AthleteResponse(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    string Profile,
    string ProfileMedium,
    DateTime CreatedAt);
