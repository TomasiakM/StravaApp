namespace Strava.Contracts.Authorization;
public record StravaRefreshTokenResponse(
    string TokenType,
    int ExpiresAt,
    int ExpiresIn,
    string RefreshToken,
    string AccessToken);