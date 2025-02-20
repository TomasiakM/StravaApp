using Achievements.Application.Features.Achievements.Commands.DeleteAllUserAchievements;
using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Microsoft.EntityFrameworkCore;

namespace Achievements.Integration.Tests.Features.Achievements.Commands;


public class DeleteAllUserAchievements : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public DeleteAllUserAchievements(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task DeleteUserAchievementsConsumerShould_DeleteAllUserAchievements_IfUserIdIsRelatedWithUser()
    {
        var userId = 5;
        var command = new DeleteAllUserAchievementsCommand(userId);

        var achievement1 = new CumulativeDistanceAchievement(userId);
        var achievement2 = new YearlyCumulativeDistanceAchievement(userId);
        await Insert(achievement1);
        await Insert(achievement2);

        await Mediator.Send(command);

        var userAchievements = await Db
            .Achievements
            .Where(e => e.StravaUserId == userId)
            .ToListAsync();

        Assert.Empty(userAchievements);
    }

    [Fact]
    public async Task DeleteUserAchievementsConsumer_ShouldNotDeleteUserAchievements_IfUserIdIsNotRelatedWithUser()
    {
        var userId = 6;
        var command = new DeleteAllUserAchievementsCommand(userId);

        var otherUserId = 7;
        var achievement3 = new CumulativeDistanceAchievement(otherUserId);
        var achievement4 = new YearlyCumulativeDistanceAchievement(otherUserId);
        await Insert(achievement3);
        await Insert(achievement4);


        await Mediator.Send(command);


        var userAchievements = await Db.Achievements
            .Where(e => e.StravaUserId == otherUserId)
            .ToListAsync();

        Assert.Equal(2, userAchievements.Count);
    }
}
