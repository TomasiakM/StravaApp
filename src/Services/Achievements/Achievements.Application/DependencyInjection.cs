using Achievements.Domain.Aggregates.Achievement.Factories;
using Achievements.Domain.Interfaces;
using Common.Application;
using Common.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Achievements.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommonApplication();

        services.AddServiceMapper();
        services.AddServiceMediatr();

        services.AddScoped<IAchievementFactory, AchievementFactory>();

        return services;
    }
}
