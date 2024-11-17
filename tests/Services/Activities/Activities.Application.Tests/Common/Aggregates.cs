using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Enums;
using Common.Domain.Models;

namespace Activities.Application.Tests.Common;
public static class Aggregates
{
    public static ActivityAggregate CreateActivity(long stravaActivityId, long stravaUserId)
    {
        return ActivityAggregate.Create(stravaActivityId, stravaUserId, "Test", "Device", SportType.Golf, true, 111, 222, 333, 444, 555,
            Speed.Create(1111, 2222), Time.Create(3333, 4444, new DateTime(2022, 1, 1), new DateTime(2022, 1, 2)),
            Watts.Create(true, 5555, 6666), Heartrate.Create(true, 7777, 8888), Map.Create(LatLng.Create(1, 2), LatLng.Create(3, 4), "poly", "summaryPoly"));
    }

    public static StreamAggregate CreateStream(ActivityId activityId)
    {
        return StreamAggregate.Create(activityId, new(), new(), new(), new(), new());
    }
}
