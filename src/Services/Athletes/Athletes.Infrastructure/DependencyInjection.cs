using Athletes.Infrastructure.Persistence;
using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Athletes.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceDbContext<ServiceDbContext>();

        return services;
    }
}
