using Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Extensions;
public static class DbContextExtensions
{
    public static IServiceCollection AddServiceDbContext<TContext>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder>? optionsAction = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        where TContext : BaseDbContext
    {
        return services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);
    }
}
