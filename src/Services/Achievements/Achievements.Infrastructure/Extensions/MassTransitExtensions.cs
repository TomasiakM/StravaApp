﻿using Achievements.Application.Consumers;
using Common.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Achievements.Infrastructure.Extensions;
internal static class MassTransitExtensions
{
    public static IServiceCollection AddServiceMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(e =>
        {
            e.SetKebabCaseEndpointNameFormatter();

            e.AddConsumer<UpdateAchievementsMessageConsumer>();
            e.AddConsumer<DeleteUserAchievementsMessageConsumer>();

            e.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                cfg.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.AutoDelete = false;
                cfg.Durable = true;

                cfg.UseConcurrencyLimit(1);

                cfg.SingleActiveConsumer = true;
                cfg.PrefetchCount = 0;

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
