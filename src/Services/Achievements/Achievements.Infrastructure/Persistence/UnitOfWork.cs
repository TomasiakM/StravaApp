using Achievements.Application.Interfaces;
using Achievements.Domain.Aggregates.Achievement;
using Achievements.Infrastructure.Persistence.Repositories;

namespace Achievements.Infrastructure.Persistence;
internal class UnitOfWork : IUnitOfWork
{
    private readonly ServiceDbContext _dbContext;

    public IAchievementRepository Achievements { get; }

    public UnitOfWork(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        Achievements = new AchievementRepository(_dbContext);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
