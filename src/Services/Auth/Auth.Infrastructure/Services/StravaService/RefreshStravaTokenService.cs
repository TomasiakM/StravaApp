using Auth.Infrastructure.Interfaces.Services.StravaService;
using Common.MessageBroker.Contracts.Authorization.RefreshStravaToken;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Services.StravaService;
internal sealed class RefreshStravaTokenService : IRefreshStravaTokenService
{
    private readonly IRequestClient<RefreshStravaTokenRequest> _client;
    private readonly ILogger<RefreshStravaTokenService> _logger;

    public RefreshStravaTokenService(IRequestClient<RefreshStravaTokenRequest> client, ILogger<RefreshStravaTokenService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<RefreshStravaTokenResponse> RefreshAsync(string refreshToken)
    {
        var response = await _client.GetResponse<RefreshStravaTokenResponse>(
            new RefreshStravaTokenRequest(refreshToken));

        return response.Message;
    }
}
