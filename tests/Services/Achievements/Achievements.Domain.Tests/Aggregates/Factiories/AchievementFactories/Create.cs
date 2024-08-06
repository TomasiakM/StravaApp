using Achievements.Domain.Aggregates.Achievement.Enums;
using Achievements.Domain.Aggregates.Achievement.Factories;

namespace Achievements.Domain.Tests.Aggregates.Factiories.AchievementFactories;
public class Create
{
    [Fact]
    public void ShouldCreateAllTypesOfAchievements()
    {
        foreach (AchievementType achievementType in Enum.GetValues(typeof(AchievementType)))
        {
            var stravaUserId = 312;
            var factory = new AchievementFactory();

            var achievement = factory.Create(stravaUserId, achievementType);

            Assert.Empty(achievement.AchievementLevels);
            Assert.Equal(stravaUserId, achievement.StravaUserId);
            Assert.Equal(achievementType, achievement.AchievementType);
        }
    }
}
