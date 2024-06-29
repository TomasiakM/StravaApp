using Common.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommonApplication();

        return services;
    }
}
