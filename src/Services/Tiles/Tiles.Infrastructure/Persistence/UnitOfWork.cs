using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Infrastructure.Persistence.Repositories;

namespace Tiles.Infrastructure.Persistence;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ServiceDbContext _dbContext;

    public IActivityTilesRepository Tiles { get; }

    public UnitOfWork(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        Tiles = new ActivityTilesRepository(dbContext);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
