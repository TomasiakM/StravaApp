using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Enums;
using Common.Domain.Models;

namespace Activities.Domain.Tests.Aggregates.Activities;
public class Update
{
    [Fact]
    public void ShouldUUpdateActivityAggregate()
    {
        var stravaId = 2;
        var stravaUserId = 5;
        var name = "test";
        var deviceName = "device";
        var sportType = SportType.Ride;
        var @private = true;
        var distance = 20300;
        var totalElevationGain = 200;
        var averageCadence = 110.0f;
        var kilojoules = 10200;
        var calories = 202;

        var speed = Speed.Create(35.4f, 29.9f);
        var time = Time.Create(2300, 5215, DateTime.UtcNow, DateTime.UtcNow.AddHours(2));
        var watts = Watts.Create(true, 222, 160);
        var heartrate = Heartrate.Create(true, 189, 167);
        var map = Map.Create(LatLng.Create(4, 2), LatLng.Create(5, 2), "testPolyline", "testSummaryPolyline");

        var activity = ActivityAggregate.Create(
            stravaId,
            stravaUserId,
            name,
            deviceName,
            sportType,
            @private,
            distance,
            totalElevationGain,
            averageCadence,
            kilojoules,
            calories,
            speed,
            time,
            watts,
            heartrate,
            map);

        var name2 = "test2";
        var deviceName2 = "device2";
        var sportType2 = SportType.Run;
        var private2 = false;
        var distance2 = 20302;
        var totalElevationGain2 = 2002;
        var averageCadence2 = 110.2f;
        var kilojoules2 = 10220;
        var calories2 = 222;

        var speed2 = Speed.Create(35.2f, 29.5f);
        var time2 = Time.Create(2302, 5212, DateTime.UtcNow, DateTime.UtcNow.AddHours(2));
        var watts2 = Watts.Create(true, 212, 164);
        var heartrate2 = Heartrate.Create(true, 183, 161);
        var map2 = Map.Create(LatLng.Create(2, 3), LatLng.Create(3, 6), "testPolyline2", "testSummaryPolyline2");

        activity.Update(
            name2,
            deviceName2,
            sportType2,
            private2,
            distance2,
            totalElevationGain2,
            averageCadence2,
            kilojoules2,
            calories2,
            speed2,
            time2,
            watts2,
            heartrate2,
            map2);

        Assert.Equal(name2, activity.Name);
        Assert.Equal(deviceName2, activity.DeviceName);
        Assert.Equal(sportType2, activity.SportType);
        Assert.Equal(private2, activity.Private);
        Assert.Equal(distance2, activity.Distance);
        Assert.Equal(totalElevationGain2, activity.TotalElevationGain);
        Assert.Equal(averageCadence2, activity.AverageCadence);
        Assert.Equal(kilojoules2, activity.Kilojoules);
        Assert.Equal(calories2, activity.Calories);
        Assert.Equal(speed2, activity.Speed);
        Assert.Equal(time2, activity.Time);
        Assert.Equal(watts2, activity.Watts);
        Assert.Equal(heartrate2, activity.Heartrate);
        Assert.Equal(map2, activity.Map);
    }
}
