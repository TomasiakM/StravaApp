using Common.Infrastructure.Persistence;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Infrastructure.Persistence.Repositories;
internal sealed class ActivityTilesRepository :
    GenericRepository<ActivityTilesAggregate, ActivityTilesId>, IActivityTilesRepository
{
    public ActivityTilesRepository(BaseDbContext dbContext)
        : base(dbContext)
    {
    }
}
