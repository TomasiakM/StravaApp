using Common.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Strava.Infrastructure.Extensions;
internal static class MassTransitExtensions
{
    public static IServiceCollection AddServiceMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(e =>
        {
            e.SetKebabCaseEndpointNameFormatter();

            e.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                cfg.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
