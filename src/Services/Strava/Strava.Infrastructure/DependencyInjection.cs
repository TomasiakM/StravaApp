using Microsoft.Extensions.DependencyInjection;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Infrastructure.Extensions;
using Strava.Infrastructure.HttpClients;
using Strava.Infrastructure.Interfaces;
using Strava.Infrastructure.Services.StravaDataServices;

namespace Strava.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddServiceSettings();
        services.AddServiceMassTransit();

        services.AddScoped<StravaHttpClientService>();
        services.AddScoped<StravaAuthenticationHttpClientService>();

        services.AddScoped<IUserActivityService, UserActivityService>();
        services.AddScoped<IActivityStreamsService, ActivityStreamsService>();
        services.AddScoped<IAllUserActivitiesService, AllUserActivitiesService>();

        services.AddScoped<IConfirmStravaAuthenticationCodeService, ConfirmStravaAuthenticationCodeService>();
        services.AddScoped<IRefreshStravaUserTokenService, RefreshStravaUserTokenService>();

        return services;
    }
}
