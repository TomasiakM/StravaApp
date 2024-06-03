using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Contracts.Activity;
using Strava.Infrastructure.HttpClients;

namespace Strava.Infrastructure.Services.StravaDataServices;
internal sealed class UserActivityService : IUserActivityService
{
    private readonly ILogger<UserActivityService> _logger;
    private readonly StravaHttpClientService _stravaHttpClientService;

    public UserActivityService(ILogger<UserActivityService> logger, StravaHttpClientService stravaHttpClientService)
    {
        _logger = logger;
        _stravaHttpClientService = stravaHttpClientService;
    }

    public async Task<StravaActivityDetailsResponse> GetAsync(long stravaUserId, long stravaActivityId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching activity:{ActifityId}, made by user:{UserId}.", stravaActivityId, stravaUserId);

        var response = await _stravaHttpClientService.GetResponse<StravaActivityDetailsResponse>(
                stravaUserId,
                $"activities/{stravaActivityId}",
                cancellationToken: cancellationToken);

        _logger.LogInformation("Success with fetching activity:{ActifityId}, made by user:{UserId}.", stravaActivityId, stravaUserId);

        return response;
    }
}
