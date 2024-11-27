using Auth.Application.Consumers;
using Auth.Application.Features.Token.Commands.Delete;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Auth.Application.Tests.Consumers.DeleteUserToken;
public class Consume
{
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<IBus> _busMock = new();

    [Fact]
    public async Task ShouldSendDeleteUserTokenCommand()
    {
        var correlationId = Guid.NewGuid();
        var userId = 1;
        var message = new DeleteUserTokenMessage(correlationId, userId);

        var consumer = new DeleteUserTokenMessageConsumer(
            _senderMock.Object,
            _busMock.Object,
            Mock.Of<ILogger<DeleteUserTokenMessageConsumer>>());

        var consumeContext = Mock.Of<ConsumeContext<DeleteUserTokenMessage>>(context =>
            context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new DeleteTokenCommand(message.StravaUserId), default), Times.Once);
        _busMock.Verify(e => e.Publish(new UserTokenDeletedEvent(message.CorrelationId, message.StravaUserId), default), Times.Once);
    }
}
