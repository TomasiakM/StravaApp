using Activities.Application.Interfaces;
using Common.MessageBroker.Contracts.Achievements;
using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class ReceivedActivityDataEventConsumer
    : IConsumer<ReceivedActivityDataEvent>
{
    private readonly ILogger<ReceivedActivityDataEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityAggregateFactory _activityAggregateFactory;
    private readonly IBus _bus;

    public ReceivedActivityDataEventConsumer(ILogger<ReceivedActivityDataEventConsumer> logger, IUnitOfWork unitOfWork, IActivityAggregateFactory activityAggregateFactory, IBus bus)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _activityAggregateFactory = activityAggregateFactory;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ReceivedActivityDataEvent> context)
    {
        _logger.LogInformation("Starting processing activity:{Id} data.", context.Message.Id);

        var activity = await _unitOfWork.Activities
            .GetAsync(e => e.StravaId == context.Message.Id);

        if (activity is null)
        {
            var newActivity = _activityAggregateFactory.CreateActivity(context.Message);

            _unitOfWork.Activities.Add(newActivity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("New activity:{Id} \"{Name}\" is created", newActivity.StravaId, newActivity.Name);
            return;
        }

        var message = context.Message;
        activity.Update(
                message.Name,
                message.DeviceName,
                message.SportType,
                message.Private,
                message.Distance,
                message.TotalElevationGain,
                message.AverageCadence,
                message.Kilojoules,
                message.Calories,
                _activityAggregateFactory.CreateSpeed(message),
                _activityAggregateFactory.CreateTime(message),
                _activityAggregateFactory.CreateWatts(message),
                _activityAggregateFactory.CreateHeartrate(message),
                _activityAggregateFactory.CreateMap(message));

        await _unitOfWork.SaveChangesAsync();

        await _bus.Publish(new UpdateAchievementsEvent(activity.StravaUserId));

        _logger.LogInformation("Activity:{Id} \"{Name}\" updated successfully.", activity.StravaId, activity.Name);
    }
}
