using Achievements.Application.Interfaces;
using Achievements.Application.Interfaces.Services;
using Achievements.Infrastructure.Extensions;
using Achievements.Infrastructure.Persistence;
using Achievements.Infrastructure.Services.Activities;
using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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

    public static async Task<WebApplication> MigrateAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetService<ServiceDbContext>();
        await dbContext!.Database.MigrateAsync();

        return app;
    }
}
