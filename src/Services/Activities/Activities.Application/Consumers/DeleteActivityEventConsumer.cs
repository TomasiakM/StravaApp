using Activities.Application.Interfaces;
using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class DeleteActivityEventConsumer : IConsumer<DeleteActivityEvent>
{
    private readonly ILogger<DeleteActivityEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteActivityEventConsumer(ILogger<DeleteActivityEventConsumer> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<DeleteActivityEvent> context)
    {
        _logger.LogInformation("Deleting activity:{ActivityId}.", context.Message.StravaActivityId);

        var activity = await _unitOfWork.Activities.FindAsync(e => e.StravaId == context.Message.StravaActivityId);

        if (activity is not null)
        {
            _unitOfWork.Activities.Delete(activity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Activity:{ActivityId} has been removed successfully.", context.Message.StravaActivityId);

            return;
        }

        _logger.LogInformation("Activity:{ActivityId} not found.", context.Message.StravaActivityId);
    }
}
