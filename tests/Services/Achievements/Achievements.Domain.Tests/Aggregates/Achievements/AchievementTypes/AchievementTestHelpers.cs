using Achievements.Domain.Aggregates.Achievement;
using Common.Domain.Interfaces;
using Common.Domain.Models;
using Moq;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes;
public static class AchievementTestHelpers
{
    public static void TestAchievement(long stravaUserId, Achievement achievement, List<Activity> activities, int expectedLevel)
    {
        var mockContext = new Mock<IDateProvider>();

        var date = new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero);
        mockContext.Setup(m => m.OffsetUtcNow).Returns(date);

        achievement.UpdateLevel(activities, mockContext.Object);

        Assert.Equal(stravaUserId, achievement.StravaUserId);
        Assert.True(achievement.AchievementLevels.Count == expectedLevel);
        Assert.True(achievement.AchievementLevels.All(e => e.AchievedAt == date));

        if (expectedLevel > 0)
        {
            var levels = achievement.AchievementLevels
                .OrderBy(e => e.Level)
                .Select(e => e.Level);

            Assert.True(levels.SequenceEqual(Enumerable.Range(1, expectedLevel)));
        }
        else
        {
            Assert.Empty(achievement.AchievementLevels);
        }
    }
}
