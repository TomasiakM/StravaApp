using Strava.Contracts.Athlete;

namespace Strava.Contracts.Authorization;
public record StravaAuthorizationResponse(
    string TokenType,
    int ExpiresAt,
    int ExpiresIn,
    string RefreshToken,
    string AccessToken,
    StravaAthleteSummaryResponse Athlete);
