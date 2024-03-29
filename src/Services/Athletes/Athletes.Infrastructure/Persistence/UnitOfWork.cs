using Athletes.Application.Interfaces;
using Athletes.Domain.Aggregates.Athletes;
using Athletes.Infrastructure.Persistence.Repositories;

namespace Athletes.Infrastructure.Persistence;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ServiceDbContext _dbContext;

    public IAthleteRepository Athletes { get; }

    public UnitOfWork(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        Athletes = new AthleteRepository(dbContext);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
