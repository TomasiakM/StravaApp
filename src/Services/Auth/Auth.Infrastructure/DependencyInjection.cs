using Auth.Application.Interfaces;
using Auth.Infrastructure.Persistence;
using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceDbContext<ServiceDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
