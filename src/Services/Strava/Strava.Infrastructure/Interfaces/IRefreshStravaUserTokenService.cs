using Strava.Contracts.Authorization;

namespace Strava.Infrastructure.Interfaces;
public interface IRefreshStravaUserTokenService
{
    Task<StravaRefreshTokenResponse> RefreshAsync(long stravaUserId, CancellationToken cancellationToken = default);
}
