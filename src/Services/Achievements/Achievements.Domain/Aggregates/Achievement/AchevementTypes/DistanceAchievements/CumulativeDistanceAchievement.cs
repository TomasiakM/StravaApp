using Achievements.Domain.Aggregates.Achievement.Enums;
using Common.Domain.Interfaces;
using Common.Domain.Models;

namespace Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
public sealed class CumulativeDistanceAchievement : Achievement
{
    private readonly List<int> s_thresholds = new()
    {
        100, 250, 500,
        1000, 2500, 5000,
        10000, 20000, 30000, 40000,
        50000, 75000, 100000
    };

    public CumulativeDistanceAchievement(long stravaUserId)
        : base(stravaUserId, AchievementType.CumulativeDistance) { }

    public override void UpdateLevel(IEnumerable<Activity> activities, IDateProvider dateProvider)
    {
        var distance = activities.Sum(a => a.Distance);

        var level = s_thresholds
            .Where(threshold => threshold <= distance)
            .Count();

        SetLevel(level, dateProvider);
    }

    public override IEnumerable<object> GetThresholds()
    {
        return s_thresholds
            .Select(e => (object)e)
            .ToList();
    }
}
