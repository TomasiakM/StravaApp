using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Common.Domain.Models;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.YearlyCumulativeDistanceAchievements;
public class UpdateLevel
{
    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldUpdateLevel(List<Activity> activities, int level)
    {
        var stravaUserId = 12;
        var achievement = new YearlyCumulativeDistanceAchievement(stravaUserId);

        AchievementTestHelpers.TestAchievement(
            stravaUserId,
            achievement,
            activities,
            level);
    }

    public static IEnumerable<object[]> Data()
    {
        yield return new object[] { new List<Activity>(), 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1499999.9, DateTime.Now) }, 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1499999.9, DateTime.Now), new(Guid.NewGuid(), 1.0, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1500000, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1999999.9, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2000000, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2499999.9, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2500000, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2999999.9, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 3000000, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 3999999.9, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 4000000, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 4999999.9, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 5000000, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 5999999.9, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 6000000, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 6999999.9, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 7000000, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 7999999.9, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 8000000, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 8999999.9, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 9000000, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 9999999.9, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 10000000, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 14999999.9, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 15000000, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 19999999.9, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 20000000, DateTime.Now) }, 13 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100000000000, DateTime.Now) }, 13 };
    }
}
