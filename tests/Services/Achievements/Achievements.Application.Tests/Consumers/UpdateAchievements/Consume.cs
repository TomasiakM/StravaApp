using Achievements.Application.Consumers;
using Achievements.Application.Features.Achievements.Commands.Calculate;
using Common.MessageBroker.Saga.Common.Events;
using Common.MessageBroker.Saga.Common.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Achievements.Application.Tests.Consumers.UpdateAchievements;
public class Consume
{
    private readonly Mock<IBus> _busMock = new();
    private readonly Mock<ISender> _senderMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldConsumeMessage(Guid correlationId, long stravaActivityId, long stravaUserId)
    {
        var message = new UpdateAchievementsMessage(correlationId, stravaActivityId, stravaUserId);

        var consumer = new UpdateAchievementsMessageConsumer(
            _senderMock.Object,
            _busMock.Object,
            Mock.Of<ILogger<UpdateAchievementsMessageConsumer>>());

        var consumeContext = Mock.Of<ConsumeContext<UpdateAchievementsMessage>>(context =>
            context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new CalculateAchievementsCommand(stravaUserId), new()), Times.Once);
        _busMock.Verify(e => e.Publish(new AchievementsUpdatedEvent(message.CorrelationId, message.StravaActivityId, message.StravaUserId), new()), Times.Once);
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[] { new Guid("4289442b-23d8-4dd8-8e3c-24e39179c625"), 6, 2 };
        yield return new object[] { new Guid("d1c1b8ef-a978-4384-8bde-0f9a4ac8782d"), 854, 65 };
    }
}
