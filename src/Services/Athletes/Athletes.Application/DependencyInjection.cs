using Common.Application;
using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Athletes.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommonApplication();

        services.AddServiceMediatr();
        services.AddServiceMapper();

        return services;
    }
}
