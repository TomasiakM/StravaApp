using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Tiles.Application.Interfaces;
using Tiles.Infrastructure.Persistence;

namespace Tiles.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceAuthentication();

        services.AddHttpContextAccessor();
        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
