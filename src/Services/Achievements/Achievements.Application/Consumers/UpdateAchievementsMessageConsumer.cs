using Achievements.Application.Features.Achievements.Commands.UpdateAchievements;
using Common.MessageBroker.Saga.Common.Events;
using Common.MessageBroker.Saga.Common.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Achievements.Application.Consumers;
public sealed class UpdateAchievementsMessageConsumer : IConsumer<UpdateAchievementsMessage>
{
    private readonly ISender _sender;
    private readonly IBus _bus;
    private readonly ILogger<UpdateAchievementsMessageConsumer> _logger;

    public UpdateAchievementsMessageConsumer(ISender sender, IBus bus, ILogger<UpdateAchievementsMessageConsumer> logger)
    {
        _sender = sender;
        _bus = bus;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateAchievementsMessage> context)
    {
        await _sender.Send(new UpdateAchievementsCommand(context.Message.StravaUserId));

        _logger.LogInformation("[BUS]: Sending {Event}", nameof(AchievementsUpdatedEvent));
        await _bus.Publish(new AchievementsUpdatedEvent(
            context.Message.CorrelationId,
            context.Message.StravaActivityId,
            context.Message.StravaUserId));
    }
}
