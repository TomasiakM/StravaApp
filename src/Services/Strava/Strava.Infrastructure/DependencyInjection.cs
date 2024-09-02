using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Infrastructure.Extensions;
using Strava.Infrastructure.HttpClients;
using Strava.Infrastructure.Interfaces;
using Strava.Infrastructure.Interfaces.Auth;
using Strava.Infrastructure.Persistence;
using Strava.Infrastructure.Services.Auth;
using Strava.Infrastructure.Services.StravaDataServices;

namespace Strava.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddServiceSettings();
        services.AddServiceMassTransit();

        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<StravaHttpClientService>();
        services.AddScoped<StravaAuthenticationHttpClientService>();

        services.AddScoped<IStravaTokenService, StravaTokenService>();

        services.AddScoped<IUserActivityService, UserActivityService>();
        services.AddScoped<IActivityStreamsService, ActivityStreamsService>();
        services.AddScoped<IAllUserActivitiesService, AllUserActivitiesService>();

        services.AddScoped<IConfirmStravaAuthenticationCodeService, ConfirmStravaAuthenticationCodeService>();
        services.AddScoped<IRefreshStravaUserTokenService, RefreshStravaUserTokenService>();

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
