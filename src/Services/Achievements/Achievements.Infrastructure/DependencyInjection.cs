using Achievements.Application.Interfaces;
using Achievements.Infrastructure.Persistence;
using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Achievements.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceDbContext<ServiceDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
