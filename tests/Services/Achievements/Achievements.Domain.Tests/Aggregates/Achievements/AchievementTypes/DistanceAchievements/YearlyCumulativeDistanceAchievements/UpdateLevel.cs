using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Common.Domain.Models;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.YearlyCumulativeDistanceAchievements;
public class UpdateLevel
{
    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldUpdateLevel(List<Activity> activities, int level)
    {
        var achievement = new YearlyCumulativeDistanceAchievement(1);
        var mockContext = new Mock<IDateProvider>();

        AchievementTestHelpers.TestAchievement(
            stravaUserId,
            achievement,
            activities,
            level);
        }

    public static IEnumerable<object[]> Data()
    {
        var activity1 = new Activity(Guid.NewGuid(), 1499.9, new DateTime(2022, 12, 31));
        var activity2 = new Activity(Guid.NewGuid(), 1499.9, new DateTime(2023, 1, 1));
        var activity3 = new Activity(Guid.NewGuid(), 499.9, new DateTime(2023, 12, 31));

        yield return new object[] { new List<Activity>(), 0 };
        yield return new object[] { new List<Activity>() { activity1 }, 0 };
        yield return new object[] { new List<Activity>() { activity1, activity2 }, 0 };
        yield return new object[] { new List<Activity>() { activity2, activity3 }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1500, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2000, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2500, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 3000, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 4000, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 5000, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 6000, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 7000, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 8000, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 9000, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 10000, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 15000, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 20000, DateTime.Now) }, 13 };
    }
}
