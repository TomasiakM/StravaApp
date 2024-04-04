using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces;
using Strava.Contracts.Activity;

namespace Strava.Infrastructure.Services;
internal sealed class StravaActivitiesService : IStravaActivitiesService
{
    private const int PageSize = 100;

    private readonly ILogger<StravaActivitiesService> _logger;
    private readonly StravaHttpClientService _stravaHttpClientService;

    public StravaActivitiesService(ILogger<StravaActivitiesService> logger, StravaHttpClientService stravaHttpClientService)
    {
        _logger = logger;
        _stravaHttpClientService = stravaHttpClientService;
    }

    public async Task<StravaActivityDetailsResponse> GetActivityDetails(long stravaUserId, long stravaActivityId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching activity:{ActifityId}, made by user:{UserId}.", stravaActivityId, stravaUserId);

        var response = await _stravaHttpClientService.GetResponse<StravaActivityDetailsResponse>(
                stravaUserId,
                $"activities/{stravaActivityId}");

        _logger.LogInformation("Success with fetching activity:{ActifityId}, made by user:{UserId}.", stravaActivityId, stravaUserId);

        return response;
    }

    public async Task<ICollection<StravaActivitySummaryResponse>> GetAthleteActivities(long stravaUserId, CancellationToken cancellationToken = default)
    {
        var page = 1;
        var lastActivitiesCount = 0;
        var activities = new List<StravaActivitySummaryResponse>();

        _logger.LogInformation("Starting feching all user:{UserId} activities.", stravaUserId);

        do
        {
            _logger.LogInformation("Fetching {Page} page with activities.", page);

            var response = await _stravaHttpClientService.GetResponse<List<StravaActivitySummaryResponse>>(
                stravaUserId,
                "athlete/activities",
                new Dictionary<string, string>()
                {
                    { "page", page.ToString() },
                    { "per_page", PageSize.ToString() },
                });

            _logger.LogInformation("Received page {Page} containing {Count} elements.", page, response.Count);

            lastActivitiesCount = response.Count;

            activities.AddRange(response);

            page++;
        } while (lastActivitiesCount == PageSize);

        return activities;
    }
}
