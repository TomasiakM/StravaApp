using Common.Domain.DDD;
using Common.Domain.Models;
using Tiles.Domain.Aggregates.Coordinates.ValueObjects;

namespace Tiles.Domain.Aggregates.Coordinates;
public sealed class CoordinatesAggregate : AggregateRoot<CoordinatesId>
{
    public long StravaActivityId { get; init; }
    public List<LatLng> Coordinates { get; private set; }

    private CoordinatesAggregate(long stravaActivityId, List<LatLng> coordinates)
        : base(CoordinatesId.Create())
    {
        StravaActivityId = stravaActivityId;
        Coordinates = coordinates;
    }

    public static CoordinatesAggregate Create(long stravaActivityId, List<LatLng> coordinates)
        => new(stravaActivityId, coordinates);

    public void Update(List<LatLng> coordinates)
    {
        Coordinates = coordinates;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CoordinatesAggregate() : base(CoordinatesId.Create()) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
