using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Models;

namespace Activities.Domain.Tests.Aggregates.Streams;
public class Update
{
    [Fact]
    public void ShouldUpdateStream()
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

        var cadence2 = new List<int>() { 1, 2, 3, 4, 5 };
        var heartrate2 = new List<int>() { 5, 6, 7, 8, 9 };
        var altitude2 = new List<float>() { 9, 10, 11, 12, 13 };
        var distance2 = new List<float>() { 13, 14, 15, 16, 17 };
        var latLng2 = new List<LatLng>() { LatLng.Create(1, 2), LatLng.Create(3, 4), LatLng.Create(5, 6), LatLng.Create(7, 8), LatLng.Create(9, 10) };

        stream.Update(cadence2, heartrate2, altitude2, distance2, latLng2);

        Assert.True(cadence2.SequenceEqual(stream.Cadence));
        Assert.True(heartrate2.SequenceEqual(stream.Heartrate));
        Assert.True(altitude2.SequenceEqual(stream.Altitude));
        Assert.True(distance2.SequenceEqual(stream.Distance));
        Assert.True(latLng2.SequenceEqual(stream.LatLngs));
    }
}
