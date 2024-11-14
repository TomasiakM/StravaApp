﻿using Achievements.Application.Consumers;
using Achievements.Application.Features.Achievements.Commands.DeleteAllUserAchievements;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Achievements.Application.Tests.Consumers.DeleteUserAchievements;
public class Consume
{
    private readonly Mock<IBus> _busMock = new();
    private readonly Mock<ISender> _senderMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldConsumeMessage(Guid correlationId, long stravaUserId)
    {
        var message = new DeleteUserAchievementsMessage(correlationId, stravaUserId);

        var consumer = new DeleteUserAchievementsMessageConsumer(
            _senderMock.Object,
            _busMock.Object,
            Mock.Of<ILogger<DeleteUserAchievementsMessageConsumer>>());

        var consumeContext = Mock.Of<ConsumeContext<DeleteUserAchievementsMessage>>(context =>
            context.Message == message);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new DeleteAllUserAchievementsCommand(message.StravaUserId), new()), Times.Once);
        _busMock.Verify(e => e.Publish(new UserAchievementsDeletedEvent(message.CorrelationId, message.StravaUserId), new()), Times.Once);
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[] { new Guid("482220b5-c772-4aa2-8c43-0ad0bc2ca376"), 63 };
        yield return new object[] { new Guid("f93eb168-58f7-430e-8dfc-030858a4cd7c"), 215 };
    }
}
