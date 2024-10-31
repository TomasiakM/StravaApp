using Auth.Application.Features.Token.Commands.Delete;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Consumers;
public sealed class DeleteUserTokenMessageConsumer : IConsumer<DeleteUserTokenMessage>
{
    private readonly ISender _sender;
    private readonly IBus _bus;
    private readonly ILogger<DeleteUserTokenMessageConsumer> _logger;

    public DeleteUserTokenMessageConsumer(ISender sender, IBus bus, ILogger<DeleteUserTokenMessageConsumer> logger)
    {
        _sender = sender;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteUserTokenMessage> context)
    {
        await _sender.Send(new DeleteTokenCommand(context.Message.StravaUserId));

        _logger.LogInformation("[BUS]: Publishing {Event}", nameof(UserTokenDeletedEvent));
        await _bus.Publish(new UserTokenDeletedEvent(
            context.Message.CorrelationId,
            context.Message.StravaUserId));
    }
}
