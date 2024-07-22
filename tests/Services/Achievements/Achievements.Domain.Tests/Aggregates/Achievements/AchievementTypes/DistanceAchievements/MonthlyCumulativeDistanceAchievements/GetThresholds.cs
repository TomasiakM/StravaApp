using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.MonthlyCumulativeDistanceAchievements;
public class GetThresholds
{
    [Fact]
    public void ShouldReturnListWithThresholds()
    {
        var achivement = new MonthlyCumulativeDistanceAchievement(1);

        Assert.Equal(14, achivement.GetThresholds().Count());
    }
}
