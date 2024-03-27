using Microsoft.Extensions.DependencyInjection;
using Strava.Application.Interfaces;
using Strava.Infrastructure.Services;
using Strava.Infrastructure.Settings;

namespace Strava.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddOptions<StravaSettings>()
            .BindConfiguration(nameof(StravaSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IStravaAuthenticationService, StravaAuthenticationService>();

        return services;
    }
}
