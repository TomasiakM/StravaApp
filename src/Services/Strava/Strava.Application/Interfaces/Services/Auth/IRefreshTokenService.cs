using Strava.Application.Dtos.Auth;

namespace Strava.Application.Interfaces.Services.Auth;
public interface IRefreshTokenService
{
    Task<RefreshTokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default);
}
