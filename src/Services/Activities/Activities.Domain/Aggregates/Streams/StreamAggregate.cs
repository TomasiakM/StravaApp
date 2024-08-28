using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams.ValueObjects;
using Common.Domain.DDD;
using Common.Domain.Models;

namespace Activities.Domain.Aggregates.Streams;
public sealed class StreamAggregate : AggregateRoot<StreamId>
{
    public ActivityId ActivityId { get; init; }

    private List<int> _cadence = new();
    public IReadOnlyList<int> Cadence => _cadence.AsReadOnly();

    private List<int> _heartrate = new();
    public IReadOnlyList<int> Heartrate => _heartrate.AsReadOnly();

    private List<float> _altitude = new();
    public IReadOnlyList<float> Altitude => _altitude.AsReadOnly();

    private List<float> _distance = new();
    public IReadOnlyList<float> Distance => _distance.AsReadOnly();

    private List<LatLng> _latLngs = new();
    public IReadOnlyList<LatLng> LatLngs => _latLngs.AsReadOnly();

    private StreamAggregate(ActivityId activityId, List<int> cadence, List<int> heartrate, List<float> altitude, List<float> distance, List<LatLng> latLngs)
        : base(StreamId.Create())
    {
        ActivityId = activityId;

        _cadence = cadence;
        _heartrate = heartrate;
        _altitude = altitude;
        _distance = distance;
        _latLngs = latLngs;
    }

    public static StreamAggregate Create(ActivityId activityId, List<int> cadence, List<int> heartrate, List<float> altitude, List<float> distance, List<LatLng> latLngs)
        => new(activityId, cadence, heartrate, altitude, distance, latLngs);

    public void Update(List<int> cadence, List<int> heartrate, List<float> altitude, List<float> distance, List<LatLng> latLngs)
    {
        _cadence = cadence;
        _heartrate = heartrate;
        _altitude = altitude;
        _distance = distance;
        _latLngs = latLngs;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private StreamAggregate() : base(StreamId.Create()) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
