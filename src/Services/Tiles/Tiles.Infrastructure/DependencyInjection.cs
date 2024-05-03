using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Tiles.Application.Interfaces;
using Tiles.Infrastructure.Extensions;
using Tiles.Infrastructure.Persistence;

namespace Tiles.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceSettings();
        services.AddServiceMassTransit();

        services.AddServiceAuthentication();

        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
