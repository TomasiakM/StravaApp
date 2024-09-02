using MassTransit;

namespace Common.MessageBroker.Saga.ProcessActivityData;
public class ProcessActivitySagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public long StravaActivityId { get; set; }
    public long StravaUserId { get; set; }

    public bool ActivitiesServiceProcessed { get; set; }
    public bool TilesServiceProcessed { get; set; }
    public bool AchievementsServiceProcessed { get; set; }
}
