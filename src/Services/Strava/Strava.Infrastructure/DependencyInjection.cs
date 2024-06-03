using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strava.Application.Interfaces;
using Strava.Application.Interfaces.Services.Auth;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Infrastructure.Extensions;
using Strava.Infrastructure.HttpClients;
using Strava.Infrastructure.Interfaces;
using Strava.Infrastructure.Persistence;
using Strava.Infrastructure.Providers;
using Strava.Infrastructure.Services;
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

        services.AddStravaServiceAuthentication();

        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<StravaHttpClientService>();
        services.AddScoped<StravaAuthenticationHttpClientService>();

        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddScoped<IUserActivityService, UserActivityService>();
        services.AddScoped<IActivityStreamsService, ActivityStreamsService>();
        services.AddScoped<IAllUserActivitiesService, AllUserActivitiesService>();

        services.AddScoped<IConfirmStravaAuthenticationCodeService, ConfirmStravaAuthenticationCodeService>();
        services.AddScoped<IRefreshStravaUserTokenService, RefreshStravaUserTokenService>();

        services.AddScoped<IUserStravaTokenProvider, UserStravaTokenProvider>();

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
