using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Contracts.Activity;
using Strava.Infrastructure.HttpClients;

namespace Strava.Infrastructure.Services.StravaDataServices;
internal class AllUserActivitiesService : IAllUserActivitiesService
{
    private const int ResponsePageSize = 100;

    private readonly ILogger<AllUserActivitiesService> _logger;
    private readonly StravaHttpClientService _stravaHttpClientService;

    public AllUserActivitiesService(ILogger<AllUserActivitiesService> logger, StravaHttpClientService stravaHttpClientService)
    {
        _logger = logger;
        _stravaHttpClientService = stravaHttpClientService;
    }

    public async Task<IEnumerable<StravaActivitySummaryResponse>> GetAsync(long stravaUserId, CancellationToken cancellationToken = default)
    {
        var page = 1;
        int lastActivitiesCount;
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
                    { "per_page", ResponsePageSize.ToString() },
                },
                cancellationToken);

            _logger.LogInformation("Received page {Page} containing {Count} elements.", page, response.Count);

            lastActivitiesCount = response.Count;

            activities.AddRange(response);

            page++;
        } while (lastActivitiesCount == ResponsePageSize);

        _logger.LogInformation("User:{UserId} - found {ActivitiesCount} activities.", stravaUserId, activities.Count);

        var orderedActivities = activities.OrderBy(e => e.StartDate);

        return orderedActivities;
    }
}
