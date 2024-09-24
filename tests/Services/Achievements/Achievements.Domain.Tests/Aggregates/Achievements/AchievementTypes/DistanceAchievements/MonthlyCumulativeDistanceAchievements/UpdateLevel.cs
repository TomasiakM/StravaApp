using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Common.Domain.Models;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.MonthlyCumulativeDistanceAchievements;
public class UpdateLevel
{
    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldUpdateLevel(List<Activity> activities, int level)
    {
        var stravaUserId = 12;
        var achievement = new MonthlyCumulativeDistanceAchievement(stravaUserId);

        AchievementTestHelpers.TestAchievement(
            stravaUserId,
            achievement,
            activities,
            level);
    }

    public static IEnumerable<object[]> Data()
    {
        yield return new object[] { new List<Activity>(), 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 99999.9, DateTime.Now) }, 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 99999.9, DateTime.Now), new(Guid.NewGuid(), 1.0, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100000, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 199999.9, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 200000, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 299999.9, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 300000, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 399999.9, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 400000, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 499999.9, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 500000, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 599999.9, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 600000, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 699999.9, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 700000, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 799999.9, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 800000, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 899999.9, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 900000, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 999999.9, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1000000, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1249999.9, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1250000, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1499999.9, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1500000, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1749999.9, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1750000, DateTime.Now) }, 13 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1999999.9, DateTime.Now) }, 13 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2000000, DateTime.Now) }, 14 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100000000, DateTime.Now) }, 14 };
    }
}
