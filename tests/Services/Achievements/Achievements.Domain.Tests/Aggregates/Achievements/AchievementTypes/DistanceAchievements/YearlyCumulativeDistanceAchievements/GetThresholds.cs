using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.YearlyCumulativeDistanceAchievements;
public class GetThresholds
{
    [Fact]
    public void ShouldReturnListWithThresholds()
    {
        var achivement = new YearlyCumulativeDistanceAchievement(1);

        Assert.Equal(13, achivement.GetThresholds().Count());
    }
}
