using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Strava.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddServiceMapper();

        return services;
    }
}
