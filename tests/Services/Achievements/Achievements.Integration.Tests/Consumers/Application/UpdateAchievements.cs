using Achievements.Application.Consumers;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities.GetUserActivities;
using Common.MessageBroker.Saga.Common.Messages;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using MassTransit;
using MassTransit.Testing;

namespace Achievements.Integration.Tests.Consumers.Application;

public class UpdateAchievements : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public UpdateAchievements(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ConsumerShould_ConsumeAndPublishEvent()
    {
        var message = new UpdateAchievementsMessage(Guid.NewGuid(), 1, 1);

        Harness.Bus.ConnectReceiveEndpoint(
            e => e.Handler<GetUserActivitiesRequest>(
                async e => await e.RespondAsync(
                    new GetUserActivitiesResponse(new List<Activity>()))));

        await Harness.Bus.Publish(message);
        await Task.Delay(50);

        var consumerHarness = Harness.GetConsumerHarness<UpdateAchievementsMessageConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<UpdateAchievementsMessage>());
        Assert.True(await Harness.Published
            .SelectAsync<UserAchievementsDeletedEvent>(e =>
                e.Context.Message.CorrelationId == message.CorrelationId &&
                e.Context.Message.StravaUserId == message.StravaUserId)
            .Any());
    }
}
