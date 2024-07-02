using Strava.Contracts.Authorization;

namespace Strava.Infrastructure.Interfaces;
public interface IRefreshStravaUserTokenService
{
    Task<StravaRefreshTokenResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
}
