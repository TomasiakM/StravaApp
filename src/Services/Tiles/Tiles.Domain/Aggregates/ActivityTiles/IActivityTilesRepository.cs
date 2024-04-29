using Common.Domain.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Aggregates.ActivityTiles;
public interface IActivityTilesRepository
    : IRepository<ActivityTilesAggregate, ActivityTilesId>
{
}
