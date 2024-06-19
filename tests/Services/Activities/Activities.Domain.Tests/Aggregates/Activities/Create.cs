using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Enums;
using Common.Domain.Models;

namespace Activities.Domain.Tests.Aggregates.Activities;
public class Create
{
    [Fact]
    public void ShouldCreateActivity()
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

        Assert.Equal(stravaId, activity.StravaId);
        Assert.Equal(stravaUserId, activity.StravaUserId);
        Assert.Equal(name, activity.Name);
        Assert.Equal(deviceName, activity.DeviceName);
        Assert.Equal(sportType, activity.SportType);
        Assert.Equal(@private, activity.Private);
        Assert.Equal(distance, activity.Distance);
        Assert.Equal(totalElevationGain, activity.TotalElevationGain);
        Assert.Equal(averageCadence, activity.AverageCadence);
        Assert.Equal(kilojoules, activity.Kilojoules);
        Assert.Equal(calories, activity.Calories);
        Assert.Equal(speed, activity.Speed);
        Assert.Equal(time, activity.Time);
        Assert.Equal(watts, activity.Watts);
        Assert.Equal(heartrate, activity.Heartrate);
        Assert.Equal(map, activity.Map);
    }
}
