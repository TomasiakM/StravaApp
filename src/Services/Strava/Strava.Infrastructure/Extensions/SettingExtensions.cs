using Common.MessageBroker.Settings;
using Microsoft.Extensions.DependencyInjection;
using Strava.Infrastructure.Settings;

namespace Strava.Infrastructure.Extensions;
internal static class SettingExtensions
{
    public static IServiceCollection AddServiceSettings(this IServiceCollection services)
    {
        services.AddOptions<StravaSettings>()
            .BindConfiguration(nameof(StravaSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<TokenSettings>()
            .BindConfiguration(nameof(TokenSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<MessageBrokerSettings>()
            .BindConfiguration(nameof(MessageBrokerSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
