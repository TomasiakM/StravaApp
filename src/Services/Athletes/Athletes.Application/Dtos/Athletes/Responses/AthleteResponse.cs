namespace Athletes.Application.Dtos.Athletes.Responses;
public record AthleteResponse(
    Guid Id,
    long StravaUserId,
    string Username,
    string Firstname,
    string Lastname,
    string Profile,
    string ProfileMedium,
    DateTime CreatedAt);
