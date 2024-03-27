using Common.Domain.DDD;
using Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Infrastructure.Persistence;
public abstract class GenericRepository<TEntity, TId>
    : IRepository<TEntity, TId>
    where TEntity : class, IAggregateRoot
    where TId : ValueObject
{
    private readonly BaseDbContext _dbContext;

    protected GenericRepository(BaseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .FindAsync(id, cancellationToken);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .Where(predicate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public void Add(TEntity entity)
    {
        _dbContext
            .Set<TEntity>()
            .Add(entity);
    }

    public void Update(TEntity entity)
    {
        _dbContext
            .Set<TEntity>()
            .Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbContext
            .Set<TEntity>()
            .Remove(entity);
    }
}
