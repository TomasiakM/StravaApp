using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;

namespace Common.MessageBroker.Saga.DeleteAllUserdData;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public sealed class DeletaAllUserDataSaga : MassTransitStateMachine<DeletaAllUserDataSagaData>
{
    public State StartingSaga { get; set; }

    public State AuthProcessed { get; set; }
    public State AthletesProcessed { get; set; }
    public State ActivityProcessed { get; set; }
    public State TilesProcessed { get; set; }
    public State AchievementsProcessed { get; set; }

    public Event<DeletaAllUserDataStartSagaMessage> DeletaAllUserDataStartSagaMessage { get; set; }

    public Event<UserTokenDeletedEvent> UserTokenDeletedEvent { get; set; }
    public Event<UserDetailsDeletedEvent> UserDetailsDeletedEvent { get; set; }
    public Event<UserActivitiesDeletedEvent> UserActivitiesDeletedEvent { get; set; }
    public Event<UserTilesDeletedEvent> UserTilesDeletedEvent { get; set; }
    public Event<UserAchievementsDeletedEvent> UserAchievementsDeletedEvent { get; set; }

    public DeletaAllUserDataSaga()
    {
        InstanceState(e => e.CurrentState);

        Event(() => DeletaAllUserDataStartSagaMessage, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => UserTokenDeletedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => UserDetailsDeletedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => UserActivitiesDeletedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
        Event(() => UserTilesDeletedEvent, e => e.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(DeletaAllUserDataStartSagaMessage)
                .Publish(context => CreateDeleteUserTokenMessage(context.Message))
                .Then(context => context.Saga.StravaUserId = context.Message.StravaUserId)
                .TransitionTo(StartingSaga));

        During(StartingSaga,
            When(UserTokenDeletedEvent)
                .Then(context => context.Saga.AuthServiceProcessed = true)
                .Publish(context => CreateDeleteUserDetailsMessage(context.Message))
                .TransitionTo(AuthProcessed));

        During(AuthProcessed,
            When(UserDetailsDeletedEvent)
                .Then(context => context.Saga.AthletesServiceProcessed = true)
                .Publish(context => CreateDeleteUserActivitiesMessage(context.Message))
                .TransitionTo(AthletesProcessed));

        During(AthletesProcessed,
            When(UserActivitiesDeletedEvent)
                .Then(context => context.Saga.ActivitiesServiceProcessed = true)
                .Publish(context => CreateDeleteUserTilesMessage(context.Message))
                .TransitionTo(ActivityProcessed));

        During(ActivityProcessed,
            When(UserTilesDeletedEvent)
                .Then(context => context.Saga.TilesServiceProcessed = true)
                .Publish(context => CreateDeleteUserAchievementsMessage(context.Message))
                .TransitionTo(TilesProcessed));

        During(TilesProcessed,
            When(UserAchievementsDeletedEvent)
                .Then(context => context.Saga.AchievementsServiceProcessed = true)
                .TransitionTo(AchievementsProcessed)
                .Finalize());
    }

    private static DeleteUserTokenMessage CreateDeleteUserTokenMessage(DeletaAllUserDataStartSagaMessage message)
    {
        return new(message.CorrelationId, message.StravaUserId);
    }

    private static DeleteUserDetailsMessage CreateDeleteUserDetailsMessage(UserTokenDeletedEvent @event)
    {
        return new(@event.CorrelationId, @event.StravaUserId);
    }

    private static DeleteUserActivitiesMessage CreateDeleteUserActivitiesMessage(UserDetailsDeletedEvent @event)
    {
        return new(@event.CorrelationId, @event.StravaUserId);
    }

    private static DeleteUserTilesMessage CreateDeleteUserTilesMessage(UserActivitiesDeletedEvent @event)
    {
        return new(@event.CorrelationId, @event.StravaUserId);
    }

    private static DeleteUserAchievementsMessage CreateDeleteUserAchievementsMessage(UserTilesDeletedEvent @event)
    {
        return new(@event.CorrelationId, @event.StravaUserId);
    }
}
