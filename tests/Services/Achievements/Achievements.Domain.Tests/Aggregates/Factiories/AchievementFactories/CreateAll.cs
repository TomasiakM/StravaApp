using Achievements.Domain.Aggregates.Achievement;
using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Achievements.Domain.Aggregates.Achievement.Factories;

namespace Achievements.Domain.Tests.Aggregates.Factiories.AchievementFactories;
public class CreateAll
{
    [Fact]
    public void ShouldCreateAllAchievements()
    {
        var stravaUserId = 12342;
        var factory = new AchievementFactory();

        var achievements = factory.CreateAll(stravaUserId);

        Assert.Equal(4, achievements.Count);
        Assert.Equal(4, achievements.DistinctBy(e => e.AchievementType).Count());
    }

    [Fact]
    public void ShouldCreateAllAchievementsExpect()
    {
        var stravaUserId = 12342;
        var factory = new AchievementFactory();

        var except = new List<Achievement>
        {
            new CumulativeDistanceAchievement(stravaUserId),
        };

        var achievements = factory.CreateAll(stravaUserId, except);

        Assert.Equal(3, achievements.Count);
        Assert.Equal(3, achievements.DistinctBy(e => e.AchievementType).Count());
    }

    [Fact]
    public void ShouldCreateAllAchievementsExpect2()
    {
        var stravaUserId = 12342;
        var factory = new AchievementFactory();

        var except = new List<Achievement>
        {
            new CumulativeDistanceAchievement(stravaUserId),
            new MaxDistanceAchievement(stravaUserId),
            new YearlyCumulativeDistanceAchievement(stravaUserId),
        };

        var achievements = factory.CreateAll(stravaUserId, except);

        Assert.Equal(1, achievements.Count);
        Assert.Single(achievements.DistinctBy(e => e.AchievementType));
    }

    [Fact]
    public void ShouldCreateAllAchievementsExpect3()
    {
        var stravaUserId = 12342;
        var factory = new AchievementFactory();

        var except = new List<Achievement>
        {
            new CumulativeDistanceAchievement(stravaUserId),
            new MaxDistanceAchievement(stravaUserId),
            new MonthlyCumulativeDistanceAchievement(stravaUserId),
            new YearlyCumulativeDistanceAchievement(stravaUserId),
        };

        var achievements = factory.CreateAll(stravaUserId, except);

        Assert.Empty(achievements);
    }
}
