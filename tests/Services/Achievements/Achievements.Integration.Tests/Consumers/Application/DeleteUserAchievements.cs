using Achievements.Application.Consumers;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;

namespace Achievements.Integration.Tests.Consumers.Application;

public class DeleteUserAchievements : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public DeleteUserAchievements(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ConsumerShould_ConsumeAndPublishEvent()
    {
        var message = new DeleteUserAchievementsMessage(Guid.NewGuid(), 1);

        await Harness.Bus.Publish(message);
        await Task.Delay(50);

        var consumerHarness = Harness.GetConsumerHarness<DeleteUserAchievementsMessageConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<DeleteUserAchievementsMessage>());
        Assert.True(await Harness.Published.Any<UserAchievementsDeletedEvent>());
    }
}
