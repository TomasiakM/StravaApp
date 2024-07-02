using Microsoft.Extensions.Logging;
using Strava.Contracts.Authorization;
using Strava.Infrastructure.HttpClients;
using Strava.Infrastructure.Interfaces;

namespace Strava.Infrastructure.Services.StravaDataServices;
internal sealed class RefreshStravaUserTokenService : IRefreshStravaUserTokenService
{
    private readonly StravaAuthenticationHttpClientService _stravaAuthenticationHttpClientService;
    private readonly ILogger<RefreshStravaUserTokenService> _logger;

    public RefreshStravaUserTokenService(StravaAuthenticationHttpClientService stravaAuthenticationHttpClientService, ILogger<RefreshStravaUserTokenService> logger)
    {
        _stravaAuthenticationHttpClientService = stravaAuthenticationHttpClientService;
        _logger = logger;
    }

    public async Task<StravaRefreshTokenResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var formData = new Dictionary<string, string>()
        {
            { "refresh_token", refreshToken },
            { "grand_type", "refresh_token"}
        };

        var response = await _stravaAuthenticationHttpClientService
            .PostRequest<StravaRefreshTokenResponse>(formData, cancellationToken);

        return response;
    }
}
