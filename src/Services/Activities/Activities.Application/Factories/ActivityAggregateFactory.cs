using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities;

namespace Activities.Application.Factories;
internal sealed class ActivityAggregateFactory : IActivityAggregateFactory
{
    public ActivityAggregate CreateActivity(ReceivedActivityDataEvent activityData)
    {

        return ActivityAggregate.Create(
                activityData.Id,
                activityData.Athlete.Id,
                activityData.Name,
                activityData.DeviceName,
                activityData.SportType,
                activityData.Private,
                activityData.Distance,
                activityData.TotalElevationGain,
                activityData.AverageCadence,
                activityData.Kilojoules,
                activityData.Calories,
                CreateSpeed(activityData),
                CreateTime(activityData),
                CreateWatts(activityData),
                CreateHeartrate(activityData),
                CreateMap(activityData));
    }

    public LatLng? CreateEndLatLng(ReceivedActivityDataEvent activityData)
    {
        if (activityData.EndLatlng.Length == 2)
        {
            return LatLng.Create(
                activityData.EndLatlng[0],
                activityData.EndLatlng[1]);
        }

        return null;
    }

    public Heartrate CreateHeartrate(ReceivedActivityDataEvent activityData)
    {
        return Heartrate.Create(
            activityData.HasHeartrate,
            activityData.MaxHeartrate,
            activityData.AverageHeartrate);
    }

    public Map CreateMap(ReceivedActivityDataEvent activityData)
    {
        return Map.Create(
            CreateStartLatLng(activityData),
            CreateEndLatLng(activityData),
            activityData.Map.Polyline,
            activityData.Map.SummaryPolyline);
    }

    public Speed CreateSpeed(ReceivedActivityDataEvent activityData)
    {
        return Speed.Create(
            activityData.MaxSpeed,
            activityData.AverageSpeed);
    }

    public LatLng? CreateStartLatLng(ReceivedActivityDataEvent activityData)
    {
        if (activityData.StartLatlng.Length == 2)
        {
            return LatLng.Create(
                activityData.StartLatlng[0],
                activityData.StartLatlng[1]);
        }

        return null;
    }

    public Time CreateTime(ReceivedActivityDataEvent activityData)
    {
        return Time.Create(
            activityData.MovingTime,
            activityData.ElapsedTime,
            activityData.StartDate,
            activityData.StartDateLocal);
    }

    public Watts CreateWatts(ReceivedActivityDataEvent activityData)
    {
        return Watts.Create(
            activityData.DeviceWatts,
            activityData.MaxWatts,
            activityData.AverageWatts);
    }
}
