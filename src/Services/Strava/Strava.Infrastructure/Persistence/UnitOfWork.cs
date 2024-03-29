using Strava.Application.Interfaces;
using Strava.Domain.Aggregates.Token;
using Strava.Infrastructure.Persistence.Repositories;

namespace Strava.Infrastructure.Persistence;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ServiceDbContext _dbContext;
    public ITokenRepository Tokens { get; }

    public UnitOfWork(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        Tokens = new TokenRepository(_dbContext);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
