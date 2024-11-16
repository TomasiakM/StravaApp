using Activities.Application.Consumers;
using Activities.Application.Features.Activities.Commands.Delete;
using Common.MessageBroker.Saga.DeleteActivity.Events;
using Common.MessageBroker.Saga.DeleteActivity.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Activities.Application.Tests.Consumers.DeleteActivity;
public class Consume
{
    private readonly Mock<IBus> _busMock = new();
    private readonly Mock<ISender> _senderMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldConsumeDeleteActivityMessage(Guid correlationId, long stravaActivityId, long stravaUserId)
    {
        var message = new DeleteActivityMessage(correlationId, stravaActivityId, stravaUserId);

        var consumer = new DeleteActivityMessageConsumer(
            Mock.Of<ILogger<DeleteActivityMessageConsumer>>(),
            _busMock.Object,
            _senderMock.Object);

        var consumeContext = Mock.Of<ConsumeContext<DeleteActivityMessage>>(context =>
            context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new DeleteActivityCommand(message.StravaActivityId), default), Times.Once);
        _busMock.Verify(e => e.Publish(new ActivityDeletedEvent(message.CorrelationId, message.StravaActivityId, message.StravaUserId), default), Times.Once);
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[] { new Guid("482220b5-c772-4aa2-8c43-0ad0bc2ca376"), 63, 46 };
        yield return new object[] { new Guid("f93eb168-58f7-430e-8dfc-030858a4cd7c"), 215, 34 };
    }
}
