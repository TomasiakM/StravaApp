using Activities.Application.Factories;
using Common.Domain.Enums;
using Common.MessageBroker.Contracts.Activities;

namespace Activities.Application.Tests.Factories.ActivityAggregate;
public class CreateActivity
{
    [Fact]
    public void ShouldCreateActivityAggregate()
    {
        var activityAggregateFactory = new ActivityAggregateFactory();

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

        var maxSpeed = 35.4f;
        var avgSpeed = 29.9f;

        var movingTime = 2300;
        var elapseTime = 5215;
        var startDate = DateTime.UtcNow;
        var startDateLocal = DateTime.UtcNow.AddHours(2);

        var deviceWatts = true;
        var maxWatts = 222;
        var avgWatts = 133.3f;

        var hasHeartrate = true;
        var avgHeartrate = 165.4f;
        var maxHeartrate = 199;

        var startLatLng = new[] { 2.3d, 65.2d };
        var endLatLng = new[] { 5.2d, 2.5d };
        var polyline = "testPolyline";
        var summaryPolyline = "testSummaryPolyline";

        var @event = new ReceivedActivityDataEvent(
            stravaId,
            name,
            distance,
            movingTime,
            elapseTime,
            totalElevationGain,
            sportType,
            startDate,
            startDateLocal,
            startLatLng,
            endLatLng,
            @private,
            avgSpeed,
            maxSpeed,
            averageCadence,
            avgWatts,
            maxWatts,
            deviceWatts,
            kilojoules,
            calories,
            deviceName,
            hasHeartrate,
            avgHeartrate,
            maxHeartrate,
            new AthleteMetaEvent(stravaUserId),
            new MapSummaryEvent("id", polyline, summaryPolyline));

        var activity = activityAggregateFactory.CreateActivity(@event);

        Assert.Equal(stravaId, activity.StravaId);
        Assert.Equal(name, activity.Name);
        Assert.Equal(distance, activity.Distance);
        Assert.Equal(movingTime, activity.Time.MovingTime);
        Assert.Equal(elapseTime, activity.Time.ElapsedTime);
        Assert.Equal(totalElevationGain, activity.TotalElevationGain);
        Assert.Equal(sportType, activity.SportType);
        Assert.Equal(startDate, activity.Time.StartDate);
        Assert.Equal(startDateLocal, activity.Time.StartDateLocal);
        Assert.Equal(startLatLng.First(), activity.Map.StartLatlng!.Latitude);
        Assert.Equal(startLatLng.Last(), activity.Map.StartLatlng.Longitude);
        Assert.Equal(endLatLng.First(), activity.Map.EndLatlng!.Latitude);
        Assert.Equal(endLatLng.Last(), activity.Map.EndLatlng.Longitude);
        Assert.Equal(@private, activity.Private);
        Assert.Equal(avgSpeed, activity.Speed.AverageSpeed);
        Assert.Equal(maxSpeed, activity.Speed.MaxSpeed);
        Assert.Equal(averageCadence, activity.AverageCadence);
        Assert.Equal(avgWatts, activity.Watts.AverageWatts);
        Assert.Equal(maxWatts, activity.Watts.MaxWatts);
        Assert.Equal(deviceWatts, activity.Watts.DeviceWatts);
        Assert.Equal(kilojoules, activity.Kilojoules);
        Assert.Equal(calories, activity.Calories);
        Assert.Equal(deviceName, activity.DeviceName);
        Assert.Equal(hasHeartrate, activity.Heartrate.HasHeartrate);
        Assert.Equal(avgHeartrate, activity.Heartrate.AverageHeartrate);
        Assert.Equal(maxHeartrate, activity.Heartrate.MaxHeartrate);
        Assert.Equal(stravaUserId, activity.StravaUserId);
        Assert.Equal(polyline, activity.Map.Polyline);
        Assert.Equal(summaryPolyline, activity.Map.SummaryPolyline);
    }
}
