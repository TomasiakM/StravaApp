using Activities.Application.Features.Activities.Commands.DeleteAllUserActivities;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class DeleteUserActivitiesMessageConsumer : IConsumer<DeleteUserActivitiesMessage>
{
    private readonly ISender _sender;
    private readonly IBus _bus;
    private readonly ILogger<DeleteUserActivitiesMessageConsumer> _logger;

    public DeleteUserActivitiesMessageConsumer(ISender sender, IBus bus, ILogger<DeleteUserActivitiesMessageConsumer> logger)
    {
        _sender = sender;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteUserActivitiesMessage> context)
    {
        await _sender.Send(new DeleteAllUserActivitiesCommand(context.Message.StravaUserId));

        _logger.LogInformation("[BUS]: Publishing {Event}.", nameof(UserActivitiesDeletedEvent));
        await _bus.Publish(new UserActivitiesDeletedEvent(
            context.Message.CorrelationId,
            context.Message.StravaUserId));
    }
}
