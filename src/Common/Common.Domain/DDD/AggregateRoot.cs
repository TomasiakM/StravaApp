using Common.Domain.Interfaces;

namespace Common.Domain.DDD;
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : notnull
{
    protected AggregateRoot(TId id)
        : base(id)
    {
    }
}