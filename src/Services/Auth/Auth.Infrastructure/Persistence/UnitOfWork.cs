using Auth.Application.Interfaces;
using Auth.Domain.Aggregates.Token;
using Auth.Infrastructure.Persistence.Repositories;

namespace Auth.Infrastructure.Persistence;
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
