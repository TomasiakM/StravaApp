﻿using Common.Infrastructure.Settings;
using Common.MessageBroker.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.Extensions;
internal static class SettingExtensions
{
    public static IServiceCollection AddServiceSettings(this IServiceCollection services)
    {
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
