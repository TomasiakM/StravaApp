using MassTransit;

namespace Common.MessageBroker.Saga.DeleteAllUserdData;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public class DeletaAllUserDataSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public long StravaUserId { get; set; }

    public bool AuthServiceProcessed { get; set; }
    public bool AthletesServiceProcessed { get; set; }
    public bool ActivitiesServiceProcessed { get; set; }
    public bool TilesServiceProcessed { get; set; }
    public bool AchievementsServiceProcessed { get; set; }
}
