using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tiles.Application.Consumers;
using Tiles.Application.Features.ActivityTiles.Commands.DeleteAllUserTiles;

namespace Tiles.Application.Tests.Consumers.DeleteUserTiles;
public class Consume
{
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<IBus> _busMock = new();

    [Fact]
    public async Task ShouldConsume()
    {
        var coorelationId = Guid.NewGuid();
        var userId = 44;
        var message = new DeleteUserTilesMessage(coorelationId, userId);

        var consumer = new DeleteUserTilesMessageConsumer(
            _senderMock.Object,
            _busMock.Object,
            Mock.Of<ILogger<DeleteUserTilesMessageConsumer>>());
        var consumeContext = Mock.Of<ConsumeContext<DeleteUserTilesMessage>>(context =>
            context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new DeleteAllUserTilesCommand(message.StravaUserId), default), Times.Once);
        _busMock.Verify(e => e.Publish(new UserTilesDeletedEvent(
            message.CorrelationId,
            message.StravaUserId), default), Times.Once);
    }
}
