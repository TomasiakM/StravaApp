namespace Common.MessageBroker.Contracts.Authorization.RefreshStravaToken;
public record RefreshStravaTokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresAt);
