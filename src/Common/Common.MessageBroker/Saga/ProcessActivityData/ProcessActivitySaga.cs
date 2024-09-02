using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit;

namespace Common.MessageBroker.Saga.ProcessActivityData;
public sealed class ProcessActivitySaga : MassTransitStateMachine<ProcessActivitySagaData>
{
    public State ActivityProcessed { get; set; }
    public State TilesProcessed { get; set; }
    public State AchievementsProcessed { get; set; }

    public Event<ProcessActivityDataMessage> ProcessActivityDataMessage { get; set; }

    public Event<ActivityProcessedEvent> ActivityProcessedEvent { get; set; }
    public Event<TilesProcessedEvent> TilesProcessedEvent { get; set; }
    public Event<AchievementsProcessedEvent> AchievementsProcessedEvent { get; set; }

    public ProcessActivitySaga()
    {
        InstanceState(e => e.CurrentState);

        Event(() => ProcessActivityDataMessage, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => ActivityProcessedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => TilesProcessedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => AchievementsProcessedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(ProcessActivityDataMessage)
                .Publish(context => CreateProcessActivityMessage(context.Message))
                .Then(context =>
                {
                    context.Saga.StravaUserId = context.Message.Athlete.Id;
                    context.Saga.StravaActivityId = context.Message.Id;

                    context.Saga.ActivitiesServiceProcessed = true;
                })
                .TransitionTo(ActivityProcessed));

        During(ActivityProcessed,
            When(ActivityProcessedEvent)
                .Publish(context => CreateProcessTilesMessage(context.Message))
                .Then(context => context.Saga.TilesServiceProcessed = true)
                .TransitionTo(TilesProcessed));

        During(TilesProcessed,
            When(AchievementsProcessedEvent)
                .Publish(context => CreateProcessAchievementsMessage(context.Message))
                .Then(context => context.Saga.AchievementsServiceProcessed = true)
                .TransitionTo(AchievementsProcessed)
                .Finalize());
    }

    private static ProcessAchievementsMessage CreateProcessAchievementsMessage(AchievementsProcessedEvent message)
    {
        return new ProcessAchievementsMessage(
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
