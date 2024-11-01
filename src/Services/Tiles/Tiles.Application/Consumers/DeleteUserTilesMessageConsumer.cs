using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Tiles.Application.Features.ActivityTiles.Commands.DeleteAllUserTiles;

namespace Tiles.Application.Consumers;
public sealed class DeleteUserTilesMessageConsumer : IConsumer<DeleteUserTilesMessage>
{
    private readonly ISender _sender;
    private readonly IBus _bus;
    private readonly ILogger<DeleteUserTilesMessageConsumer> _logger;

    public DeleteUserTilesMessageConsumer(ISender sender, IBus bus, ILogger<DeleteUserTilesMessageConsumer> logger)
    {
        _sender = sender;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteUserTilesMessage> context)
    {
        await _sender.Send(new DeleteAllUserTilesCommand(context.Message.StravaUserId));

        _logger.LogInformation("[BUS]: Publishing {Event}", nameof(UserTilesDeletedEvent));
        await _bus.Publish(new UserTilesDeletedEvent(
            context.Message.CorrelationId,
            context.Message.StravaUserId));
    }
}
