﻿using Common.MessageBroker.Saga.DeleteActivity;
using Common.MessageBroker.Saga.ProcessActivityData;
using Common.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Strava.Application.Consumers;
using Strava.Infrastructure.Consumers;
using Strava.Infrastructure.Persistence;

namespace Strava.Infrastructure.Extensions;
internal static class MassTransitExtensions
{
    public static IServiceCollection AddServiceMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(e =>
        {
            e.SetKebabCaseEndpointNameFormatter();

            e.AddConsumer<NewAthleteLoggedInEventConsumer>();
            e.AddConsumer<FetchAthleteActivityEventConsumer>();

            e.AddConsumer<AuthorizeCodeConsumer>();
            e.AddConsumer<RefreshStravaTokenConsumer>();

            e.AddSagaStateMachine<ProcessActivitySaga, ProcessActivitySagaData>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<ServiceDbContext>();

                    r.UseSqlServer();
                });

            e.AddSagaStateMachine<DeleteActivitySaga, DeleteActivitySagaData>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<ServiceDbContext>();

                    r.UseSqlServer();
                });

            e.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                cfg.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                cfg.UseMessageRetry(r => r.Immediate(3));

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
