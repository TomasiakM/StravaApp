using Common.Infrastructure.Extensions;
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

        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<IStravaAuthenticationService, StravaAuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
