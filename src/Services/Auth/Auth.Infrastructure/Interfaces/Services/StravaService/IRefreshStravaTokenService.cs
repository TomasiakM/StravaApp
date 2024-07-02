using Common.MessageBroker.Contracts.Authorization.RefreshStravaToken;

namespace Auth.Infrastructure.Interfaces.Services.StravaService;
internal interface IRefreshStravaTokenService
{
    Task<RefreshStravaTokenResponse> RefreshAsync(string refreshToken);
}
