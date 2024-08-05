using Activities.Application.Interfaces;
using Common.MessageBroker.Contracts.Achievements;
using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class DeleteActivityEventConsumer : IConsumer<DeleteActivityEvent>
{
    private readonly ILogger<DeleteActivityEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBus _bus;

    public DeleteActivityEventConsumer(ILogger<DeleteActivityEventConsumer> logger, IUnitOfWork unitOfWork, IBus bus)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<DeleteActivityEvent> context)
    {
        _logger.LogInformation("Deleting activity:{ActivityId}.", context.Message.StravaActivityId);

        var activity = await _unitOfWork.Activities
            .GetAsync(e => e.StravaId == context.Message.StravaActivityId);

        if (activity is not null)
        {
            _unitOfWork.Activities.Delete(activity);
            await _unitOfWork.SaveChangesAsync();

            await _bus.Publish(new UpdateAchievementsEvent(activity.StravaUserId));

            _logger.LogInformation("Activity:{ActivityId} has been removed successfully.", context.Message.StravaActivityId);

            return;
        }

        _logger.LogInformation("Activity:{ActivityId} not found.", context.Message.StravaActivityId);
    }
}
