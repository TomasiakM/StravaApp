using Activities.Application.Features.Activities.Commands.Delete;
using Activities.Application.Interfaces;
using Common.MessageBroker.Saga.DeleteActivity.Events;
using Common.MessageBroker.Saga.DeleteActivity.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class DeleteActivityEventConsumer : IConsumer<DeleteActivityMessage>
{
    private readonly ILogger<DeleteActivityEventConsumer> _logger;
    private readonly IBus _bus;
    private readonly ISender _sender;
    public DeleteActivityEventConsumer(ILogger<DeleteActivityEventConsumer> logger, IUnitOfWork unitOfWork, IBus bus, ISender sender)
    {
        _logger = logger;
        _bus = bus;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<DeleteActivityMessage> context)
    {
        await _sender.Send(new DeleteActivityCommand(context.Message.StravaActivityId));

        _logger.LogInformation("[BUS]: Sending {Event}", nameof(ActivityDeletedEvent));
        await _bus.Publish(new ActivityDeletedEvent(
            context.Message.CorrelationId,
            context.Message.StravaActivityId,
            context.Message.StravaUserId));
    }
}
