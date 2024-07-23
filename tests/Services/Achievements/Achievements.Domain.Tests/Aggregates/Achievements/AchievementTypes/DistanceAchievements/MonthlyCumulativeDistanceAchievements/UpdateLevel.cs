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
        var activity1 = new Activity(Guid.NewGuid(), 99.9, new DateTime(2022, 1, 31));
        var activity2 = new Activity(Guid.NewGuid(), 49.9, new DateTime(2022, 2, 1));
        var activity3 = new Activity(Guid.NewGuid(), 59.9, new DateTime(2022, 2, 27));

        yield return new object[] { new List<Activity>(), 0 };
        yield return new object[] { new List<Activity>() { activity1 }, 0 };
        yield return new object[] { new List<Activity>() { activity1, activity2 }, 0 };
        yield return new object[] { new List<Activity>() { activity2, activity3 }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 199.9, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 200, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 299.9, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 300, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 399.9, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 400, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 499.9, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 500, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 599.9, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 600, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 699.9, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 700, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 799.9, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 800, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 899.9, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 900, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 999.9, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1000, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1249.9, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1250, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1499.9, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1500, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1749.9, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1750, DateTime.Now) }, 13 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1999.9, DateTime.Now) }, 13 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2000, DateTime.Now) }, 14 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100000000, DateTime.Now) }, 14 };
    }
}
