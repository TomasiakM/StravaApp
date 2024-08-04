using Achievements.Application.Interfaces;
using Achievements.Application.Interfaces.Services;
using Achievements.Infrastructure.Extensions;
using Achievements.Infrastructure.Persistence;
using Achievements.Infrastructure.Services.Activities;
using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Achievements.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceDbContext<ServiceDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserActivitiesService, UserActivitiesService>();

        services.AddServiceSettings();
        services.AddServiceMassTransit();

        services.AddServiceAuthentication();

        return services;
    }
}
