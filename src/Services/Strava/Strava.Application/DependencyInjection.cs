using Common.Application;
using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Strava.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommonApplication();

        services.AddServiceMapper();
        services.AddServiceMediatr();

        return services;
    }
}
