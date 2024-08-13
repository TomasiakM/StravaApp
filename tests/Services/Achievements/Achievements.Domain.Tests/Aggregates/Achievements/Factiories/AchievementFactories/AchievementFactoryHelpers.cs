using Achievements.Domain.Aggregates.Achievement.Enums;
using Achievements.Domain.Aggregates.Achievement.Factories;

namespace Achievements.Domain.Tests.Aggregates.Achievements.Factiories.AchievementFactories;
public static class AchievementFactoryHelpers
{
    public static void TestFactoryCreate(AchievementType achievementType)
    {
        var stravaUserId = 312;
        var factory = new AchievementFactory();

        var achievement = factory.Create(stravaUserId, achievementType);

        Assert.Empty(achievement.AchievementLevels);
        Assert.Equal(stravaUserId, achievement.StravaUserId);
        Assert.Equal(achievementType, achievement.AchievementType);
    }
}
