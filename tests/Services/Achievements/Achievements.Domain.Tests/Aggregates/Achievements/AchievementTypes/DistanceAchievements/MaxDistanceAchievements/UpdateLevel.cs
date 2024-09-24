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
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 24999.9, DateTime.Now) }, 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 24999.9, DateTime.Now), new(Guid.NewGuid(), 22.9, DateTime.Now) }, 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 25000, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 49999.9, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 50000, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 74999.9, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 75000, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 99999.9, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100000, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 149999.9, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 150000, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 199999.9, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 200000, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 249999.9, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 250000, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 299999.9, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 300000, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 399999.9, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 400000, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 499999.9, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 500000, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1000000000, DateTime.Now) }, 10 };
    }
}
