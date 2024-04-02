using Athletes.Application.Interfaces;
using Athletes.Infrastructure.Extensions;
using Athletes.Infrastructure.Persistence;
using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Athletes.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceSettings();
        services.AddServiceMassTransit();

        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
