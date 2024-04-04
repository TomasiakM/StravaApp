using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Interfaces;

namespace Activities.Domain.Aggregates.Activities;
public interface IActivityRepository
    : IRepository<ActivityAggregate, ActivityId>
{
}
