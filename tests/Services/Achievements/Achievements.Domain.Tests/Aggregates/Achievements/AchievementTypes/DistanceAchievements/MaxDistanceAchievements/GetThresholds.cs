using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.MaxDistanceAchievements;
public class GetThresholds
{
    [Fact]
    public void ShouldReturnListWithThresholds()
    {
        var achivement = new MaxDistanceAchievement(1);

        Assert.Equal(10, achivement.GetThresholds().Count());
    }
}
