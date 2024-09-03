using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;

namespace Activities.Application.Interfaces;
public interface IActivityAggregateFactory
{
    ActivityAggregate CreateActivity(ProcessActivityMessage activityData);
    Speed CreateSpeed(ProcessActivityMessage activityData);
    Time CreateTime(ProcessActivityMessage activityData);
    Watts CreateWatts(ProcessActivityMessage activityData);
    Heartrate CreateHeartrate(ProcessActivityMessage activityData);
    LatLng? CreateStartLatLng(ProcessActivityMessage activityData);
    LatLng? CreateEndLatLng(ProcessActivityMessage activityData);
    Map CreateMap(ProcessActivityMessage activityData);
}
