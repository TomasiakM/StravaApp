using Microsoft.Extensions.DependencyInjection;
using Strava.Application.Interfaces;
using Strava.Infrastructure.Interfaces;
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

        services.AddOptions<TokenSettings>()
            .BindConfiguration(nameof(TokenSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IStravaAuthenticationService, StravaAuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
