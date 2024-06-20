using Common.Infrastructure.Persistence;
using Tiles.Domain.Aggregates.Coordinates;
using Tiles.Domain.Aggregates.Coordinates.ValueObjects;

namespace Tiles.Infrastructure.Persistence.Repositories;
internal sealed class CoordinatesRepository
    : GenericRepository<CoordinatesAggregate, CoordinatesId>, ICoordinatesRepository
{
    public CoordinatesRepository(ServiceDbContext dbContext)
        : base(dbContext)
    {
    }
}
