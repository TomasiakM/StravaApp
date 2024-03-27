using Strava.Application.Dtos.Auth;
using Strava.Domain.Aggregates.Token;

namespace Strava.Application.Interfaces;
public interface IStravaAuthenticationService
{
    Task<AuthResponse> LoginAsync(AuthRequest request, CancellationToken cancellationToken = default);
    Task<TokenAggregate?> RefreshToken(long stravaUserId, CancellationToken cancellationToken = default);
}
