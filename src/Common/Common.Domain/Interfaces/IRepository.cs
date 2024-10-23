using Common.Domain.DDD;
using Common.Domain.Enums;
using System.Linq.Expressions;

namespace Common.Domain.Interfaces;
public interface IRepository<TEntity, TId>
    where TEntity : IAggregateRoot
    where TId : ValueObject
{
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        SortOrder sortOrder = SortOrder.Asc,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        SortOrder sortOrder = SortOrder.Asc,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
