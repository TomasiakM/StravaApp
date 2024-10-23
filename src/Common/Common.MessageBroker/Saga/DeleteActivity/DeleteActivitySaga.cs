using Common.MessageBroker.Saga.DeleteActivity.Events;
using Common.MessageBroker.Saga.DeleteActivity.Messages;
using MassTransit;

namespace Common.MessageBroker.Saga.DeleteActivity;
public class DeleteActivitySaga : MassTransitStateMachine<DeleteActivitySagaData>
{
    public State StartingSaga { get; set; }
    public State ActivityProcessed { get; set; }
    public State TilesProcessed { get; set; }
    public State AchievementsProcessed { get; set; }

    public Event<DeleteActivityDataMessage> DeleteActivityDataMessage { get; set; }

    public Event<ActivityDeletedEvent> ActivityDeletedEvent { get; set; }
    public Event<TilesDeletedEvent> TilesDeletedEvent { get; set; }
    public Event<AchievementsUpdatedEvent> AchievementsUpdatedEvent { get; set; }

    public DeleteActivitySaga()
    {
        InstanceState(e => e.CurrentState);

        Event(() => DeleteActivityDataMessage, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => ActivityDeletedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => TilesDeletedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => AchievementsUpdatedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(DeleteActivityDataMessage)
                .Publish(context => CreateDeleteActivityMessage(context.Message))
                .Then(context =>
                {
                    context.Saga.StravaUserId = context.Message.StravaUserId;
                    context.Saga.StravaActivityId = context.Message.StravaActivityId;
                })
                .TransitionTo(StartingSaga));

        During(StartingSaga,
            When(ActivityDeletedEvent)
                .Then(context => context.Saga.AchievementsServiceProcessed = true)
                .Publish(context => CreateDeleteTilesMessage(context.Message))
                .TransitionTo(ActivityProcessed));

        During(ActivityProcessed,
            When(TilesDeletedEvent)
                .Then(context => context.Saga.TilesServiceProcessed = true)
                .Publish(context => CreateUpdateAchievementsMessage(context.Message))
                .TransitionTo(TilesProcessed));

        During(TilesProcessed,
            When(AchievementsUpdatedEvent)
                .Then(context => context.Saga.AchievementsServiceProcessed = true)
                .TransitionTo(AchievementsProcessed)
                .Finalize());
    }

    private static DeleteActivityMessage CreateDeleteActivityMessage(DeleteActivityDataMessage message)
    {
        return new(message.CorrelationId, message.StravaActivityId, message.StravaUserId);
    }

    private static DeleteTilesMessage CreateDeleteTilesMessage(ActivityDeletedEvent @event)
    {
        return new(@event.CorrelationId, @event.StravaActivityId, @event.StravaUserId);
    }

    private static UpdateAchievementsMessage CreateUpdateAchievementsMessage(TilesDeletedEvent @event)
    {
        return new(@event.CorrelationId, @event.StravaActivityId, @event.StravaUserId);
    }
}


