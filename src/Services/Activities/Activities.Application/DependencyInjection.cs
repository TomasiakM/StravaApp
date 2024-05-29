using Activities.Application.Factories;
using Activities.Application.Interfaces;
using Common.Application;
using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Activities.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommonApplication();

        services.AddServiceMapper();
        services.AddServiceMediatr();

        services.AddScoped<IActivityAggregateFactory, ActivityAggregateFactory>();

        return services;
    }
}
