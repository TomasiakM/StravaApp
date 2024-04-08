using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Athletes.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddServiceMediatr();
        services.AddServiceMapper();

        return services;
    }
}
