
using Achievements.Domain.Aggregates.Achievement.ValueObjects;

namespace Achievements.Domain.Tests.Aggregates.ValueObjects.AchievementIds;
public class Create
{
    [Fact]
    public void ShouldCreateAchievementId()
    {
        var achievementId = AchievementId.Create();

        Assert.NotEqual(Guid.Empty, achievementId.Value);
    }

    [Fact]
    public void ShouldCreateAchievementId2()
    {
        var guid = Guid.NewGuid();
        var achievementId = AchievementId.Create(guid);

        Assert.Equal(guid, achievementId.Value);
    }

    [Fact]
    public void ShouldCreateUniqueAchievementId()
    {
        var achievementId1 = AchievementId.Create();
        var achievementId2 = AchievementId.Create();

        Assert.NotEqual(achievementId1, achievementId2);
        Assert.NotEqual(achievementId1.Value, achievementId2.Value);
    }
}
