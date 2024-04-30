using Microsoft.Extensions.DependencyInjection;

namespace Tiles.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        return services;
    }
}
