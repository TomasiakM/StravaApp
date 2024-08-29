using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Streams;
using Activities.Infrastracture.Persistence.Repositories;

namespace Activities.Infrastracture.Persistence;
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ServiceDbContext _dbContext;

    public IActivityRepository Activities { get; }
    public IStreamRepository Streams { get; }

    public UnitOfWork(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;

        Activities = new ActivityRepository(_dbContext);
        Streams = new StreamRepository(_dbContext);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
