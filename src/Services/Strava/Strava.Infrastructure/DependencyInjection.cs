using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strava.Application.Interfaces;
using Strava.Infrastructure.Extensions;
using Strava.Infrastructure.Interfaces;
using Strava.Infrastructure.Persistence;
using Strava.Infrastructure.Services;

namespace Strava.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddServiceSettings();
        services.AddServiceMassTransit();

        services.AddHttpContextAccessor();

        services.AddStravaServiceAuthentication();

        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<StravaHttpClientService>();

        services.AddScoped<IStravaAuthenticationService, StravaAuthenticationService>();
        services.AddScoped<IStravaActivitiesService, StravaActivitiesService>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

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
