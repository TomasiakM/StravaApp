using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Infrastructure.Persistence;

namespace Activities.Infrastracture.Persistence.Repositories;
internal sealed class ActivityRepository
    : GenericRepository<ActivityAggregate, ActivityId>, IActivityRepository
{
    public ActivityRepository(BaseDbContext dbContext)
        : base(dbContext)
    {
    }
}
