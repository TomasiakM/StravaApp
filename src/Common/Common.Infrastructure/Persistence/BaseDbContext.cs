using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Persistence;
public class BaseDbContext : DbContext
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
