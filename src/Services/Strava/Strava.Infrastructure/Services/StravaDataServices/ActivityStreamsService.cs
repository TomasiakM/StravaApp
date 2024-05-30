using Common.Domain.Models;
using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Application.Models;
using Strava.Contracts.Activity;

namespace Strava.Infrastructure.Services.StravaDataServices;
internal sealed class ActivityStreamsService : IActivityStreamsService
{
    private readonly ILogger<ActivityStreamsService> _logger;
    private readonly StravaHttpClientService _stravaHttpClientService;

    public ActivityStreamsService(ILogger<ActivityStreamsService> logger, StravaHttpClientService stravaHttpClientService)
    {
        _logger = logger;
        _stravaHttpClientService = stravaHttpClientService;
    }

    public async Task<ActivityStreams> GetAsync(long stravaUserId, long stravaActivityId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting feching activity:{ActivityId} streams.", stravaActivityId);

        var response = await _stravaHttpClientService.GetResponse<List<StravaActivityStreamResponse>>(
                stravaUserId,
                $"activities/{stravaActivityId}/streams",
                new Dictionary<string, string>()
                {
                    { "keys", "latlng,distance,altitude,heartrate,cadence,watts" },
                },
                cancellationToken);

        _logger.LogInformation("Activity:{ActivityId} streams fetched successfully.", stravaActivityId);

        var streams = new ActivityStreams(
            GetLatLngs(response));

        return streams;
    }

    private static List<LatLng> GetLatLngs(List<StravaActivityStreamResponse> activityStreams)
    {
        var gpxCoordinats = new List<LatLng>();
        var gpxCoordinatsStream = activityStreams.FirstOrDefault(e => e.Type == StreamType.Latlng);

        if (gpxCoordinatsStream is not null)
        {
            gpxCoordinatsStream.Data.ForEach(e =>
            {
                LatLng latlng = LatLng.Create(
                    double.Parse(e[0].GetRawText()),
                    double.Parse(e[1].GetRawText()));

                gpxCoordinats.Add(latlng);
            });
        }

        return gpxCoordinats;
    }
}
