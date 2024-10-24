using Activities.Application.Features.Activities.Commands.Add;
using Activities.Application.Features.Activities.Commands.Update;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;

namespace Activities.Application.Interfaces;
public interface IActivityAggregateFactory
{
    ActivityAggregate CreateActivity(AddActivityCommand activityData);
    Speed CreateSpeed(AddActivityCommand activityData);
    Time CreateTime(AddActivityCommand activityData);
    Watts CreateWatts(AddActivityCommand activityData);
    Heartrate CreateHeartrate(AddActivityCommand activityData);
    LatLng? CreateStartLatLng(AddActivityCommand activityData);
    LatLng? CreateEndLatLng(AddActivityCommand activityData);
    Map CreateMap(AddActivityCommand activityData);

    Speed CreateSpeed(UpdateActivityCommand activityData);
    Time CreateTime(UpdateActivityCommand activityData);
    Watts CreateWatts(UpdateActivityCommand activityData);
    Heartrate CreateHeartrate(UpdateActivityCommand activityData);
    LatLng? CreateStartLatLng(UpdateActivityCommand activityData);
    LatLng? CreateEndLatLng(UpdateActivityCommand activityData);
    Map CreateMap(UpdateActivityCommand activityData);
}
