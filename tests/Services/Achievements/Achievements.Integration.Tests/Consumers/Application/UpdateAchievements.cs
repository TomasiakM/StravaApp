using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Achievements.Domain.Aggregates.Achievement.Enums;
using Common.Application.Providers;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities.GetUserActivities;
using Common.MessageBroker.Saga.Common.Events;
using Common.MessageBroker.Saga.Common.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Achievements.Integration.Tests.Consumers.Application;
public class UpdateAchievements : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    // TODO: Tests works fine when you run them separatly, but when all those tests are launched, only first one is passing
    // Setting up response in every test do nothing, each test will have data configurated in first running test (sometimes last test can receive data from second test)
    // Problem with setting up data in Harness.Bus.ConnectReceiveEndpoint
    public UpdateAchievements(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Should_CreateAllAchievements_WithInitialLevel()
    {
        var message = new UpdateAchievementsMessage(Guid.NewGuid(), 1, 22);
        Harness.Bus.ConnectReceiveEndpoint(
            e => e.Handler<GetUserActivitiesRequest>(
                async e => await e.RespondAsync(
                    new GetUserActivitiesResponse(new List<Activity>()))));


        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<AchievementsUpdatedEvent>());
        await Task.Delay(50);

        Harness.ForceInactive();

        var achievements = await Db
            .Achievements
            .Where(e => e.StravaUserId == message.StravaUserId)
            .ToListAsync();

        Assert.NotEmpty(achievements);
        Assert.True(achievements.All(e => e.AchievementLevels.Count == 0));
    }

    [Fact]
    public async Task Should_CreateAllAchievements_WithRightLevel()
    {
        var message = new UpdateAchievementsMessage(Guid.NewGuid(), 1, 33);
        var activities = new List<Activity>
        {
             new (Guid.NewGuid(), 2000000, new(2022, 1, 1)),
        };

        Harness.Bus.ConnectReceiveEndpoint(
           e => e.Handler<GetUserActivitiesRequest>(
               async e => await e.RespondAsync(
                   new GetUserActivitiesResponse(activities))));


        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<AchievementsUpdatedEvent>());
        await Task.Delay(50);

        Harness.ForceInactive();

        var achievements = await Db
            .Achievements
            .Where(e => e.StravaUserId == message.StravaUserId)
            .ToListAsync();

        Assert.NotEmpty(achievements);

        var first = achievements.Where(e => e.AchievementType == AchievementType.CumulativeDistance).First();
        var second = achievements.Where(e => e.AchievementType == AchievementType.YearlyCumulativeDistance).First();
        Assert.Equal(4, first.AchievementLevels.Count);
        Assert.Equal(2, second.AchievementLevels.Count);
    }

    [Fact]
    public async Task Should_UpdateAllAchievements_WithRightLevel()
    {
        var message = new UpdateAchievementsMessage(Guid.NewGuid(), 1, 36);
        var activities = new List<Activity>
        {
             new (Guid.NewGuid(), 2000000, new(2022, 1, 1)),
        };

        var achievement = new CumulativeDistanceAchievement(message.StravaActivityId);
        var achievement2 = new YearlyCumulativeDistanceAchievement(message.StravaActivityId);
        achievement.UpdateLevel(activities, new DateProvider());
        achievement2.UpdateLevel(activities, new DateProvider());
        await Insert(achievement);
        await Insert(achievement2);


        activities.Add(new(Guid.NewGuid(), 2000000, new(2022, 1, 2)));
        Harness.Bus.ConnectReceiveEndpoint(
           e => e.Handler<GetUserActivitiesRequest>(
               async e => await e.RespondAsync(
                   new GetUserActivitiesResponse(activities))));


        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<AchievementsUpdatedEvent>());
        await Task.Delay(50);

        var achievements = await Db
            .Achievements
            .Where(e => e.StravaUserId == message.StravaUserId)
            .ToListAsync();

        Assert.NotEmpty(achievements);

        var first = achievements.Where(e => e.AchievementType == achievement.AchievementType).First();
        var second = achievements.Where(e => e.AchievementType == achievement2.AchievementType).First();
        Assert.Equal(5, first.AchievementLevels.Count);
        Assert.Equal(5, second.AchievementLevels.Count);
    }
}
