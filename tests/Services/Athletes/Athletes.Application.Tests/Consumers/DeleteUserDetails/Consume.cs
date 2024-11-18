using Athletes.Application.Consumers;
using Athletes.Application.Features.Athletes.Commands.Delete;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Athletes.Application.Tests.Consumers.DeleteUserDetails;
public class Consume
{
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<IBus> _busMock = new();

    [Fact]
    public async Task ShouldConsumeDeleteUserDetailsMessage()
    {
        var stravaUserId = 4;
        var correlationId = Guid.NewGuid();

        var message = new DeleteUserDetailsMessage(correlationId, stravaUserId);

        var consumer = new DeleteUserDetailsMessageConsumer(
            _senderMock.Object,
            _busMock.Object,
            Mock.Of<ILogger<DeleteUserDetailsMessageConsumer>>());

        var consumeContext = Mock.Of<ConsumeContext<DeleteUserDetailsMessage>>(
            context => context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(It.IsAny<DeleteAthleteCommand>(), default), Times.Once);
        _busMock.Verify(e => e.Publish(It.IsAny<UserDetailsDeletedEvent>(), default), Times.Once);
    }
}
