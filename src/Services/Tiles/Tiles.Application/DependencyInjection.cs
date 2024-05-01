using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Tiles.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddServiceMapper();
        services.AddServiceMediatr();

        return services;
    }
}
