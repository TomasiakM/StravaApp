using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Common.Domain.Models;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.MaxDistanceAchievements;
public class UpdateLevel
{
    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldUpdateLevel(List<Activity> activities, int level)
    {
        var stravaUserId = 12;
        var achievement = new MaxDistanceAchievement(stravaUserId);

        AchievementTestHelpers.TestAchievement(
            stravaUserId,
            achievement,
            activities,
            level);
    }

    public static IEnumerable<object[]> Data()
    {
        yield return new object[] { new List<Activity>(), 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 24.9, DateTime.Now) }, 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 24.9, DateTime.Now), new(Guid.NewGuid(), 22.9, DateTime.Now) }, 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 25, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 49.9, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 50, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 74.9, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 75, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 99.9, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 149.9, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 150, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 199.9, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 200, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 249.9, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 250, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 299.9, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 300, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 399.9, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 400, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 499.9, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 500, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1000000, DateTime.Now) }, 10 };
    }
}
