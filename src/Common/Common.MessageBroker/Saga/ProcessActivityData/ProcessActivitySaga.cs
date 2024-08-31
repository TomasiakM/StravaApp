using Common.MessageBroker.Saga.ProcessActivityData.Events;
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
                .Then(context =>
                {
                    context.Saga.StravaUserId = context.Message.Athlete.Id;
                    context.Saga.StravaActivityId = context.Message.Id;

                    context.Saga.ActivitiesServiceProcessed = true;
                })
                .Publish(context =>
                    new ActivityProcessedEvent(
                        context.Message.CorrelationId,
                        context.Message.Id,
                        context.Message.Athlete.Id,
                        context.Message.StartDate,
                        context.Message.Streams.LatLngs))
                .TransitionTo(ActivityProcessed));

        During(ActivityProcessed,
            When(ActivityProcessedEvent)
                .Then(context => context.Saga.TilesServiceProcessed = true)
                .Publish(context =>
                    new TilesProcessedEvent(
                        context.Message.CorrelationId,
                        context.Message.StravaActivityId,
                        context.Message.StravaUserId))
                .TransitionTo(TilesProcessed));

        During(TilesProcessed,
            When(AchievementsProcessedEvent)
                .Then(context => context.Saga.AchievementsServiceProcessed = true)
                .Publish(context =>
                    new AchievementsProcessedEvent(
                        context.Message.CorrelationId,
                        context.Message.StravaActivityId,
                        context.Message.StravaUserId))
                .TransitionTo(AchievementsProcessed)
                .Finalize());
    }
}
