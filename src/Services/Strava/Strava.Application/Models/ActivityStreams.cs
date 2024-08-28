using Common.Domain.Models;

namespace Strava.Application.Models;
public record ActivityStreams(
    List<int> Watts,
    List<int> Cadence,
    List<int> Heartrate,
    List<float> Altitude,
    List<float> Distance,
    List<LatLng> LatLngs);
