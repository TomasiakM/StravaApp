using Common.Infrastructure.Settings;

namespace Gateway.Api.Extensions;

public static class SettingExtensions
{
    public static IServiceCollection AddGatewaySettings(this IServiceCollection services)
    {
        services.AddOptions<TokenSettings>()
            .BindConfiguration(nameof(TokenSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
