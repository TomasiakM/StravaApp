using Common.MessageBroker.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Activities.Infrastracture.Extensions;
internal static class SettingsExtensions
{
    public static IServiceCollection AddServiceSettings(this IServiceCollection services)
    {
        services.AddOptions<MessageBrokerSettings>()
            .BindConfiguration(nameof(MessageBrokerSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
