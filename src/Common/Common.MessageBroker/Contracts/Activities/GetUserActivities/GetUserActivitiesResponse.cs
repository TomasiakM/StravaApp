using Common.Domain.Models;

namespace Common.MessageBroker.Contracts.Activities.GetUserActivities;
public record GetUserActivitiesResponse(
    IEnumerable<Activity> Activities);
