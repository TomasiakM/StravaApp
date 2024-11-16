using Activities.Application.Consumers;
using Activities.Application.Features.Activities.Commands.DeleteAllUserActivities;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Activities.Application.Tests.Consumers.DeleteUserActivities;
public class Consume
{
    private readonly Mock<IBus> _busMock = new();
    private readonly Mock<ISender> _senderMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldConsumeDeleteUserActivitiesMessage(Guid correlationId, long stravaUserId)
    {
        var message = new DeleteUserActivitiesMessage(correlationId, stravaUserId);

        var consumer = new DeleteUserActivitiesMessageConsumer(
            _senderMock.Object,
            _busMock.Object,
            Mock.Of<ILogger<DeleteUserActivitiesMessageConsumer>>());

        var consumeContext = Mock.Of<ConsumeContext<DeleteUserActivitiesMessage>>(context =>
            context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new DeleteAllUserActivitiesCommand(message.StravaUserId), default), Times.Once);
        _busMock.Verify(e => e.Publish(new UserActivitiesDeletedEvent(message.CorrelationId, message.StravaUserId), default), Times.Once);
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[] { new Guid("482220b5-c772-4aa2-8c43-0ad0bc2ca376"), 46 };
        yield return new object[] { new Guid("f93eb168-58f7-430e-8dfc-030858a4cd7c"), 215 };
    }
}
