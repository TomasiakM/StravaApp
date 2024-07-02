using Auth.Application.Dtos;

namespace Auth.Application.Interfaces.Services;
public interface IRefreshTokenService
{
    Task<RefreshTokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default);
}
