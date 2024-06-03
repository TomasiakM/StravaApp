using Microsoft.Extensions.Logging;
using Strava.Contracts.Authorization;
using Strava.Infrastructure.HttpClients;
using Strava.Infrastructure.Interfaces;

namespace Strava.Infrastructure.Services.StravaDataServices;
internal sealed class ConfirmStravaAuthenticationCodeService : IConfirmStravaAuthenticationCodeService
{
    private readonly StravaAuthenticationHttpClientService _stravaAuthenticationHttpClientService;
    private readonly ILogger<ConfirmStravaAuthenticationCodeService> _logger;

    public ConfirmStravaAuthenticationCodeService(StravaAuthenticationHttpClientService stravaAuthenticationHttpClienService, ILogger<ConfirmStravaAuthenticationCodeService> logger)
    {
        _stravaAuthenticationHttpClientService = stravaAuthenticationHttpClienService;
        _logger = logger;
    }

    public async Task<StravaAuthorizationResponse> AuthorizeAsync(string code, CancellationToken cancellationToken = default)
    {
        var formData = new Dictionary<string, string>() {
            { "code", code },
            { "grand_type", "authorization_code" }
        };

        _logger.LogInformation("Sending authorization request.");

        var response = await _stravaAuthenticationHttpClientService
            .PostRequest<StravaAuthorizationResponse>(formData, cancellationToken);

        _logger.LogInformation("Successfully authorized {Username}:{UserId}.", response.Athlete.Username, response.Athlete.Id);

        return response;
    }
}
