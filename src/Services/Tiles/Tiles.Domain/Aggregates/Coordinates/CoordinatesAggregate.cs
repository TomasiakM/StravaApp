using Common.Domain.DDD;
using Common.Domain.Models;
using Tiles.Domain.Aggregates.Coordinates.ValueObjects;

namespace Tiles.Domain.Aggregates.Coordinates;
public sealed class CoordinatesAggregate : AggregateRoot<CoordinatesId>
{
    public long StravaActivityId { get; init; }

    private List<LatLng> _latLngs = new();
    public IReadOnlyList<LatLng> LatLngs => _latLngs.AsReadOnly();

    private CoordinatesAggregate(long stravaActivityId, List<LatLng> latLngs)
        : base(CoordinatesId.Create())
    {
        StravaActivityId = stravaActivityId;
        _latLngs = latLngs;
    }

    public static CoordinatesAggregate Create(long stravaActivityId, List<LatLng> latLngs)
        => new(stravaActivityId, latLngs);

    public void Update(List<LatLng> latLngs)
    {
        _latLngs = latLngs;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CoordinatesAggregate() : base(CoordinatesId.Create()) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
