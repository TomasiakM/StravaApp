namespace Common.MessageBroker.Contracts.Authorization.AuthorizeUserCode;
public record AuthorizeUserCodeResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresAt,
    UserResponse Athlete);

public record UserResponse(
    long Id,
    string Username,
    string Firstname,
    string Lastname,
    DateTime CreatedAt,
    string ProfileMedium,
    string Profile);
