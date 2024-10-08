﻿using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Common.Domain.Models;

namespace Achievements.Domain.Tests.Aggregates.Achievements.AchievementTypes.DistanceAchievements.CumulativeDistanceAchivements;
public class UpdateLevel
{
    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldUpdateLevel(List<Activity> activities, int level)
    {
        var stravaUserId = 132;
        var achievement = new CumulativeDistanceAchievement(stravaUserId);

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
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 98999.9, DateTime.Now), new(Guid.NewGuid(), 1000.0, DateTime.Now) }, 0 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 99999.9, DateTime.Now), new(Guid.NewGuid(), 1.0, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100000, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 249999.9, DateTime.Now) }, 1 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 250000, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 499999.9, DateTime.Now) }, 2 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 500000, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 999999.9, DateTime.Now) }, 3 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1000000, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2499999.9, DateTime.Now) }, 4 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 2500000, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 4999999.9, DateTime.Now) }, 5 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 5000000, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 9999999.9, DateTime.Now) }, 6 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 10000000, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 19999999.9, DateTime.Now) }, 7 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 20000000, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 29999999.9, DateTime.Now) }, 8 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 30000000, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 39999999.9, DateTime.Now) }, 9 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 40000000, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 49999999.9, DateTime.Now) }, 10 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 50000000, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 74999999.9, DateTime.Now) }, 11 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 75000000, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 99999999.9, DateTime.Now) }, 12 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 100000000, DateTime.Now) }, 13 };
        yield return new object[] { new List<Activity>() { new(Guid.NewGuid(), 1000000000000, DateTime.Now) }, 13 };
    }
}
