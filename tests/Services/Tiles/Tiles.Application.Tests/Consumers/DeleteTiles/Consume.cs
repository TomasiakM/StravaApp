using Common.MessageBroker.Saga.DeleteActivity.Events;
using Common.MessageBroker.Saga.DeleteActivity.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tiles.Application.Consumers;
using Tiles.Application.Features.ActivityTiles.Commands.Delete;

namespace Tiles.Application.Tests.Consumers.DeleteTiles;
public class Consume
{
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<IBus> _busMock = new();

    [Fact]
    public async Task ShouldConsumeDeleteTilesMessage()
    {
        var correlationId = Guid.NewGuid();
        var activityId = 5;
        var userId = 4;

        var message = new DeleteTilesMessage(correlationId, activityId, userId);

        var consumer = new DeleteTilesMessageConsumer(
            _senderMock.Object,
            _busMock.Object,
            Mock.Of<ILogger<DeleteTilesMessageConsumer>>());

        var consumeContext = Mock.Of<ConsumeContext<DeleteTilesMessage>>(context =>
            context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new DeleteActivityTilesCommand(message.StravaActivityId), default), Times.Once);
        _busMock.Verify(e => e.Publish(new TilesDeletedEvent(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId), default), Times.Once);
    }
}
