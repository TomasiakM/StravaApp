namespace Common.Domain.Interfaces;
public interface IBaseUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
