﻿using Common.MessageBroker.Saga.Common.Events;
using Common.MessageBroker.Saga.Common.Messages;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit;

namespace Common.MessageBroker.Saga.ProcessActivityData;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public sealed class ProcessActivitySaga : MassTransitStateMachine<ProcessActivitySagaData>
{
    public State StartingSaga { get; set; }
    public State ActivityProcessed { get; set; }
    public State TilesProcessed { get; set; }
    public State AchievementsProcessed { get; set; }

    public Event<ProcessActivityDataMessage> ProcessActivityDataMessage { get; set; }

    public Event<ActivityProcessedEvent> ActivityProcessedEvent { get; set; }
    public Event<TilesProcessedEvent> TilesProcessedEvent { get; set; }
    public Event<AchievementsUpdatedEvent> AchievementsUpdatedEvent { get; set; }

    public ProcessActivitySaga()
    {
        InstanceState(e => e.CurrentState);

        Event(() => ProcessActivityDataMessage, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => ActivityProcessedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => TilesProcessedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => AchievementsUpdatedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(ProcessActivityDataMessage)
                .Publish(context => CreateProcessActivityMessage(context.Message))
                .Then(context =>
                {
                    context.Saga.StravaUserId = context.Message.Athlete.Id;
                    context.Saga.StravaActivityId = context.Message.Id;
                })
                .TransitionTo(StartingSaga));

        During(StartingSaga,
            When(ActivityProcessedEvent)
                .Then(context => context.Saga.AchievementsServiceProcessed = true)
                .Publish(context => CreateProcessTilesMessage(context.Message))
                .TransitionTo(ActivityProcessed));

        During(ActivityProcessed,
            When(TilesProcessedEvent)
                .Then(context => context.Saga.TilesServiceProcessed = true)
                .Publish(context => CreateProcessAchievementsMessage(context.Message))
                .TransitionTo(TilesProcessed));

        During(TilesProcessed,
            When(AchievementsUpdatedEvent)
                .Then(context => context.Saga.AchievementsServiceProcessed = true)
                .TransitionTo(AchievementsProcessed)
                .Finalize());
    }

    private static UpdateAchievementsMessage CreateProcessAchievementsMessage(TilesProcessedEvent message)
    {
        return new UpdateAchievementsMessage(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId);
    }

    private static ProcessTilesMessage CreateProcessTilesMessage(ActivityProcessedEvent message)
    {
        return new ProcessTilesMessage(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId,
            message.CreatedAt,
            message.SportType,
            message.LatLngs);
    }

    private static ProcessActivityMessage CreateProcessActivityMessage(ProcessActivityDataMessage message)
    {
        return new ProcessActivityMessage(
            message.CorrelationId,
            message.Id,
            message.Name,
            message.Distance,
            message.MovingTime,
            message.ElapsedTime,
            message.TotalElevationGain,
            message.SportType,
            message.StartDate,
            message.StartDateLocal,
            message.StartLatlng,
            message.EndLatlng,
            message.Private,
            message.AverageSpeed,
            message.MaxSpeed,
            message.AverageCadence,
            message.AverageWatts,
            message.MaxWatts,
            message.DeviceWatts,
            message.Kilojoules,
            message.Calories,
            message.DeviceName,
            message.HasHeartrate,
            message.AverageHeartrate,
            message.MaxHeartrate,
            new AthleteMetaMessage(message.Athlete.Id),
            new MapSummaryMessage(message.Map.Id, message.Map.Polyline, message.Map.SummaryPolyline),
            new StreamsMessage(message.Streams.Watts, message.Streams.Cadence, message.Streams.Heartrate, message.Streams.Altitude, message.Streams.Distance, message.Streams.LatLngs));
    }
}
