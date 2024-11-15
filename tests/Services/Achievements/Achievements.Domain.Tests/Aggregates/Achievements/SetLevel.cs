using Achievements.Domain.Aggregates.Achievement.Enums;
using Common.Domain.Interfaces;
using Moq;

namespace Achievements.Domain.Tests.Aggregates.Achievements;
public class SetLevel
{
    private readonly Mock<IDateProvider> _dateProviderMock = new();

    [Theory]
    [InlineData(-1)]
    [InlineData(-1111)]
    [InlineData(int.MinValue)]
    public void ShouldThrowExceptionWhenLevelIsLessThan0(int level)
    {
        var stravaUserId = 4;
        var testAchievement = new TestAchievement(stravaUserId, AchievementType.YearlyCumulativeDistance);

        Assert.Throws<ArgumentException>(() => testAchievement.SetLevelTest(level, _dateProviderMock.Object));
    }

    [Theory]
    [InlineData(TestAchievement.MaxLevel + 1)]
    [InlineData(TestAchievement.MaxLevel + 354)]
    public void ShouldThrowExceptionWhenLevelIsGreaterThanThresholdsCount(int level)
    {
        var stravaUserId = 4;
        var testAchievement = new TestAchievement(stravaUserId, AchievementType.YearlyCumulativeDistance);

        Assert.Throws<ArgumentException>(() => testAchievement.SetLevelTest(level, _dateProviderMock.Object));
    }

    [Theory]
    [InlineData(TestAchievement.MaxLevel, 2)]
    [InlineData(TestAchievement.MaxLevel, 3)]
    public void ShouldDecreaseAchievementLevel(int level, int levelToSet)
    {
        var stravaUserId = 4;
        var testAchievement = new TestAchievement(stravaUserId, AchievementType.YearlyCumulativeDistance);

        testAchievement.SetLevelTest(level, _dateProviderMock.Object);

        Assert.True(testAchievement.AchievementLevels.Count == level);
        Assert.True(testAchievement.AchievementLevels.Max(e => e.Level) == level);

        testAchievement.SetLevelTest(levelToSet, _dateProviderMock.Object);

        Assert.True(testAchievement.AchievementLevels.Count == levelToSet);
        Assert.True(testAchievement.AchievementLevels.Max(e => e.Level) == levelToSet);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(0, TestAchievement.MaxLevel)]
    [InlineData(4, TestAchievement.MaxLevel)]
    [InlineData(TestAchievement.MaxLevel, 2)]
    public void ShouldSetLevelsFrom1ToN(int level, int levelToSet)
    {
        var stravaUserId = 4;
        var testAchievement = new TestAchievement(stravaUserId, AchievementType.YearlyCumulativeDistance);

        testAchievement.SetLevelTest(level, _dateProviderMock.Object);
        testAchievement.SetLevelTest(levelToSet, _dateProviderMock.Object);

        for (int i = 1; i <= levelToSet; i++)
        {
            Assert.Contains(i, testAchievement.AchievementLevels.Select(e => e.Level));
        }
    }

    [Theory]
    [InlineData(TestAchievement.MaxLevel, 2)]
    [InlineData(TestAchievement.MaxLevel, 3)]
    [InlineData(TestAchievement.MaxLevel, 4)]
    public void ShouldNotChangeNewDateWhenDecreasingLevel(int level, int levelToSet)
    {
        var stravaUserId = 4;
        var testAchievement = new TestAchievement(stravaUserId, AchievementType.YearlyCumulativeDistance);

        var date = new DateTimeOffset(2022, 1, 1, 12, 0, 0, TimeSpan.Zero);
        _dateProviderMock.Setup(e => e.OffsetUtcNow).Returns(date);

        testAchievement.SetLevelTest(level, _dateProviderMock.Object);

        Assert.True(testAchievement.AchievementLevels.All(e => e.AchievedAt == date));


        var date2 = new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero);
        _dateProviderMock.Setup(e => e.OffsetUtcNow).Returns(date2);

        testAchievement.SetLevelTest(levelToSet, _dateProviderMock.Object);

        Assert.True(testAchievement.AchievementLevels.All(e => e.AchievedAt == date));
        Assert.True(testAchievement.AchievementLevels.All(e => e.AchievedAt != date2));
    }
}
