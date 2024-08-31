using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData;

namespace Activities.Application.Interfaces;
public interface IActivityAggregateFactory
{
    ActivityAggregate CreateActivity(ProcessActivityDataMessage activityData);
    Speed CreateSpeed(ProcessActivityDataMessage activityData);
    Time CreateTime(ProcessActivityDataMessage activityData);
    Watts CreateWatts(ProcessActivityDataMessage activityData);
    Heartrate CreateHeartrate(ProcessActivityDataMessage activityData);
    LatLng? CreateStartLatLng(ProcessActivityDataMessage activityData);
    LatLng? CreateEndLatLng(ProcessActivityDataMessage activityData);
    Map CreateMap(ProcessActivityDataMessage activityData);
}
