using Common.Application.Helpers;
using Common.Application.Interfaces;
using Common.Application.Providers;
using Common.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddCommonApplication(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IUserIdProvider, UserIdProvider>();
        services.AddScoped<IDateProvider, DateProvider>();

        return services;
    }
}
