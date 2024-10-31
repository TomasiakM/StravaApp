using Athletes.Application.Features.Athletes.Commands.Delete;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Athletes.Application.Consumers;
public sealed class DeleteUserDetailsMessageConsumer : IConsumer<DeleteUserDetailsMessage>
{
    private readonly ISender _sender;
    private readonly IBus _bus;
    private readonly ILogger<DeleteUserDetailsMessageConsumer> _logger;

    public DeleteUserDetailsMessageConsumer(ISender sender, IBus bus, ILogger<DeleteUserDetailsMessageConsumer> logger)
    {
        _sender = sender;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteUserDetailsMessage> context)
    {
        await _sender.Send(new DeleteAthleteCommand(context.Message.StravaUserId));

        _logger.LogInformation("[BUS]: Publishing {Event}", nameof(UserDetailsDeletedEvent));
        await _bus.Publish(new UserDetailsDeletedEvent(
            context.Message.CorrelationId,
            context.Message.StravaUserId));
    }
}
