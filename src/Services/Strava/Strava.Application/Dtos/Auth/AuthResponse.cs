using Strava.Application.Dtos.Athlete;

namespace Strava.Application.Dtos.Auth;
public record AuthResponse(
    string AccessToken,
    AthleteSummitResponse Athlete);
