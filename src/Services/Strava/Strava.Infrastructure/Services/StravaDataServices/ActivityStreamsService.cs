using Common.Domain.Models;
using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Application.Models;
using Strava.Contracts.Activity;
using Strava.Infrastructure.HttpClients;

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
            GetWatts(response),
            GetCadence(response),
            GetHeartrate(response),
            GetAltitude(response),
            GetDistance(response),
            GetLatLngs(response));

        return streams;
    }

    private static List<int> GetWatts(List<StravaActivityStreamResponse> activityStreams)
    {
        var watts = new List<int>();
        var wattsStream = activityStreams.FirstOrDefault(e => e.Type == StreamType.Watts);

        if (wattsStream is not null)
        {
            wattsStream.Data.ForEach(e => watts.Add(int.Parse(e.GetRawText())));
        }

        return watts;
    }

    private static List<int> GetCadence(List<StravaActivityStreamResponse> activityStreams)
    {
        var cadence = new List<int>();
        var cadenceStream = activityStreams.FirstOrDefault(e => e.Type == StreamType.Cadence);

        if (cadenceStream is not null)
        {
            cadenceStream.Data.ForEach(e => cadence.Add(int.Parse(e.GetRawText())));
        }

        return cadence;
    }

    private static List<int> GetHeartrate(List<StravaActivityStreamResponse> activityStreams)
    {
        var heartrate = new List<int>();
        var heartrateStream = activityStreams.FirstOrDefault(e => e.Type == StreamType.Heartrate);

        if (heartrateStream is not null)
        {
            heartrateStream.Data.ForEach(e => heartrate.Add(int.Parse(e.GetRawText())));
        }

        return heartrate;
    }

    private static List<float> GetAltitude(List<StravaActivityStreamResponse> activityStreams)
    {
        var altitude = new List<float>();
        var altitudeStream = activityStreams.FirstOrDefault(e => e.Type == StreamType.Altitude);

        if (altitudeStream is not null)
        {
            altitudeStream.Data.ForEach(e => altitude.Add(float.Parse(e.GetRawText())));
        }

        return altitude;
    }

    private static List<float> GetDistance(List<StravaActivityStreamResponse> activityStreams)
    {
        var distance = new List<float>();
        var distanceStream = activityStreams.FirstOrDefault(e => e.Type == StreamType.Distance);

        if (distanceStream is not null)
        {
            distanceStream.Data.ForEach(e => distance.Add(float.Parse(e.GetRawText())));
        }

        return distance;
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
