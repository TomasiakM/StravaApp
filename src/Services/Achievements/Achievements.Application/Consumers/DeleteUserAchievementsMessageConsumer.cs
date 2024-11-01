using Achievements.Application.Features.Achievements.Commands.DeleteAllUserAchievements;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Achievements.Application.Consumers;
public sealed class DeleteUserAchievementsMessageConsumer : IConsumer<DeleteUserAchievementsMessage>
{
    private readonly ISender _sender;
    private readonly IBus _bus;
    private readonly ILogger<DeleteUserAchievementsMessageConsumer> _logger;

    public DeleteUserAchievementsMessageConsumer(ISender sender, IBus bus, ILogger<DeleteUserAchievementsMessageConsumer> logger)
    {
        _sender = sender;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteUserAchievementsMessage> context)
    {
        await _sender.Send(new DeleteAllUserAchievementsCommand(context.Message.StravaUserId));

        _logger.LogInformation("[BUS]: Publishing {Event}", nameof(UserAchievementsDeletedEvent));
        await _bus.Publish(new UserAchievementsDeletedEvent(
            context.Message.CorrelationId,
            context.Message.StravaUserId));
    }
}
