using Activities.Application.Features.Activities.Commands.Add;
using Activities.Application.Features.Activities.Commands.Update;
using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;

namespace Activities.Application.Factories;
internal sealed class ActivityAggregateFactory : IActivityAggregateFactory
{
    public ActivityAggregate CreateActivity(AddActivityCommand activityData)
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

    public LatLng? CreateEndLatLng(AddActivityCommand activityData)
    {
        if (activityData.EndLatlng.Length == 2)
        {
            return LatLng.Create(
                activityData.EndLatlng[0],
                activityData.EndLatlng[1]);
        }

        return null;
    }

    public LatLng? CreateEndLatLng(UpdateActivityCommand activityData)
    {
        if (activityData.EndLatlng.Length == 2)
        {
            return LatLng.Create(
                activityData.EndLatlng[0],
                activityData.EndLatlng[1]);
        }

        return null;
    }

    public Heartrate CreateHeartrate(AddActivityCommand activityData)
    {
        return Heartrate.Create(
            activityData.HasHeartrate,
            activityData.MaxHeartrate,
            activityData.AverageHeartrate);
    }

    public Heartrate CreateHeartrate(UpdateActivityCommand activityData)
    {
        return Heartrate.Create(
            activityData.HasHeartrate,
            activityData.MaxHeartrate,
            activityData.AverageHeartrate);
    }

    public Map CreateMap(AddActivityCommand activityData)
    {
        return Map.Create(
            CreateStartLatLng(activityData),
            CreateEndLatLng(activityData),
            activityData.Map.Polyline,
            activityData.Map.SummaryPolyline);
    }

    public Map CreateMap(UpdateActivityCommand activityData)
    {
        return Map.Create(
            CreateStartLatLng(activityData),
            CreateEndLatLng(activityData),
            activityData.Map.Polyline,
            activityData.Map.SummaryPolyline);
    }

    public Speed CreateSpeed(AddActivityCommand activityData)
    {
        return Speed.Create(
            activityData.MaxSpeed,
            activityData.AverageSpeed);
    }

    public Speed CreateSpeed(UpdateActivityCommand activityData)
    {
        return Speed.Create(
            activityData.MaxSpeed,
            activityData.AverageSpeed);
    }

    public LatLng? CreateStartLatLng(AddActivityCommand activityData)
    {
        if (activityData.StartLatlng.Length == 2)
        {
            return LatLng.Create(
                activityData.StartLatlng[0],
                activityData.StartLatlng[1]);
        }

        return null;
    }

    public LatLng? CreateStartLatLng(UpdateActivityCommand activityData)
    {
        if (activityData.StartLatlng.Length == 2)
        {
            return LatLng.Create(
                activityData.StartLatlng[0],
                activityData.StartLatlng[1]);
        }

        return null;
    }

    public Time CreateTime(AddActivityCommand activityData)
    {
        return Time.Create(
            activityData.MovingTime,
            activityData.ElapsedTime,
            activityData.StartDate,
            activityData.StartDateLocal);
    }

    public Time CreateTime(UpdateActivityCommand activityData)
    {
        return Time.Create(
            activityData.MovingTime,
            activityData.ElapsedTime,
            activityData.StartDate,
            activityData.StartDateLocal);
    }

    public Watts CreateWatts(AddActivityCommand activityData)
    {
        return Watts.Create(
            activityData.DeviceWatts,
            activityData.MaxWatts,
            activityData.AverageWatts);
    }

    public Watts CreateWatts(UpdateActivityCommand activityData)
    {
        return Watts.Create(
            activityData.DeviceWatts,
            activityData.MaxWatts,
            activityData.AverageWatts);
    }
}
