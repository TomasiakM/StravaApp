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

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        Domain.Enums.SortOrder sortOrder = Domain.Enums.SortOrder.Asc,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext
            .Set<TEntity>()
            .AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            if (sortOrder == Domain.Enums.SortOrder.Asc)
            {
                query = query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(orderBy);
            }
        }

        if (asSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        Domain.Enums.SortOrder sortOrder = Domain.Enums.SortOrder.Asc,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext
            .Set<TEntity>()
            .AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            if (sortOrder == Domain.Enums.SortOrder.Asc)
            {
                query = query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(orderBy);
            }
        }

        if (asSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<TEntity>()
            .AnyAsync(filter, cancellationToken);
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
