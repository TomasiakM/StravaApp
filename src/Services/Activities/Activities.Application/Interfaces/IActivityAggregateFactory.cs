using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities;

namespace Activities.Application.Interfaces;
public interface IActivityAggregateFactory
{
    ActivityAggregate CreateActivity(ReceivedActivityDataEvent activityData);
    Speed CreateSpeed(ReceivedActivityDataEvent activityData);
    Time CreateTime(ReceivedActivityDataEvent activityData);
    Watts CreateWatts(ReceivedActivityDataEvent activityData);
    Heartrate CreateHeartrate(ReceivedActivityDataEvent activityData);
    LatLng? CreateStartLatLng(ReceivedActivityDataEvent activityData);
    LatLng? CreateEndLatLng(ReceivedActivityDataEvent activityData);
    Map CreateMap(ReceivedActivityDataEvent activityData);
}
