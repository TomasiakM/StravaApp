using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Models;

namespace Activities.Domain.Tests.Aggregates.Streams;
public class Create
{
    [Fact]
    public void ShouldCreateStream()
    {
        var activityId = ActivityId.Create();
        var cadence = new List<int>() { 1, 2, 3, 4 };
        var heartrate = new List<int>() { 5, 6, 7, 8 };
        var altitude = new List<float>() { 9, 10, 11, 12 };
        var distance = new List<float>() { 13, 14, 15, 16 };
        var latLng = new List<LatLng>() { LatLng.Create(1, 2), LatLng.Create(3, 4), LatLng.Create(5, 6), LatLng.Create(7, 8) };

        var stream = StreamAggregate.Create(
            activityId,
            cadence,
            heartrate,
            altitude,
            distance,
            latLng);

        Assert.Equal(activityId, stream.ActivityId);
        Assert.True(cadence.SequenceEqual(stream.Cadence));
        Assert.True(heartrate.SequenceEqual(stream.Heartrate));
        Assert.True(altitude.SequenceEqual(stream.Altitude));
        Assert.True(distance.SequenceEqual(stream.Distance));
        Assert.True(latLng.SequenceEqual(stream.LatLngs));
    }
}
