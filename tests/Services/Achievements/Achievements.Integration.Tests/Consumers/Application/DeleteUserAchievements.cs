using Achievements.Application.Consumers;
using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using Microsoft.EntityFrameworkCore;

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

        var consumerHarness = Harness.GetConsumerHarness<DeleteUserAchievementsMessageConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<DeleteUserAchievementsMessage>());
        Assert.True(await Harness.Published.Any<UserAchievementsDeletedEvent>());
    }

    [Fact]
    public async Task DeleteUserAchievementsConsumerShould_DeleteAllUserAchievements()
    {
        var userId = 5;
        var message = new DeleteUserAchievementsMessage(Guid.NewGuid(), userId);

        var achievement1 = new CumulativeDistanceAchievement(userId);
        var achievement2 = new YearlyCumulativeDistanceAchievement(userId);
        await Insert(achievement1);
        await Insert(achievement2);

        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<UserAchievementsDeletedEvent>());
        await Task.Delay(50);

        var userAchievements = await Db
            .Achievements
            .Where(e => e.StravaUserId == userId)
            .ToListAsync();

        Assert.Empty(userAchievements);
    }

    [Fact]
    public async Task DeleteUserAchievementsConsumer_ShouldNot_DeleteUserAchievements_WhichHaveDifferentId()
    {
        var userId = 6;
        var message = new DeleteUserAchievementsMessage(Guid.NewGuid(), userId);

        var achievement1 = new CumulativeDistanceAchievement(userId);
        var achievement2 = new YearlyCumulativeDistanceAchievement(userId);
        await Insert(achievement1);
        await Insert(achievement2);

        var otherUserId = 7;
        var achievement3 = new CumulativeDistanceAchievement(otherUserId);
        var achievement4 = new YearlyCumulativeDistanceAchievement(otherUserId);
        await Insert(achievement3);
        await Insert(achievement4);

        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<UserAchievementsDeletedEvent>());

        await Task.Delay(50);

        var userAchievements = await Db
            .Achievements
            .Where(e => e.StravaUserId == otherUserId)
            .ToListAsync();

        var deletedUserAchievements = await Db
            .Achievements
            .Where(e => e.StravaUserId == userId)
            .ToListAsync();

        Assert.Empty(deletedUserAchievements);
        Assert.Equal(2, userAchievements.Count);
    }
}
