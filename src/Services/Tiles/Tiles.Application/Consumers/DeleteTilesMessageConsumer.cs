using Common.MessageBroker.Saga.DeleteActivity.Events;
using Common.MessageBroker.Saga.DeleteActivity.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Tiles.Application.Features.ActivityTiles.Commands.Delete;

namespace Tiles.Application.Consumers;
public sealed class DeleteTilesMessageConsumer : IConsumer<DeleteTilesMessage>
{
    private readonly ISender _sender;
    private readonly IBus _bus;
    private readonly ILogger<DeleteTilesMessageConsumer> _logger;

    public DeleteTilesMessageConsumer(ISender sender, IBus bus, ILogger<DeleteTilesMessageConsumer> logger)
    {
        _sender = sender;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteTilesMessage> context)
    {
        await _sender.Send(new DeleteActivityTilesCommand(context.Message.StravaActivityId));

        _logger.LogInformation("[BUS]: Publishing {Event}", nameof(TilesDeletedEvent));
        await _bus.Publish(new TilesDeletedEvent(
            context.Message.CorrelationId,
            context.Message.StravaActivityId,
            context.Message.StravaUserId));
    }
}
