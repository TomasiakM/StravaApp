using Common.Application;
using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Tiles.Application.Utils.ReceivedActivityTrackDetailsEventUtils;

namespace Tiles.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommonApplication();

        services.AddServiceMapper();
        services.AddServiceMediatr();

        services.AddScoped<ExistingActivityTilesHandler>();
        services.AddScoped<NewActivityTilesHandler>();

        return services;
    }
}
