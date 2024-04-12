using Strava.Application.Dtos.Auth;
using Strava.Domain.Aggregates.Token;

namespace Strava.Application.Interfaces;
public interface IStravaAuthenticationService
{
    Task<AuthResponse> LoginAsync(AuthRequest request, CancellationToken cancellationToken = default);
    Task<RefreshTokenResponse> RefreshToken();
    Task<TokenAggregate?> GetStravaUserToken(long stravaUserId, CancellationToken cancellationToken = default);
}
