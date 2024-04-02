using Common.MessageBroker.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Athletes.Infrastructure.Extensions;
internal static class SettingExtensions
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
