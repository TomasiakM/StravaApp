using Common.Domain.Models;

namespace Strava.Application.Models;
public record ActivityStreams(
    List<LatLng> LatLngs);
