using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Activities.Application;
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
