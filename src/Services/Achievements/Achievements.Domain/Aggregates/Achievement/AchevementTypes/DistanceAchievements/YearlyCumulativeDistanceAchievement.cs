using Achievements.Domain.Aggregates.Achievement.Enums;
using Common.Domain.Interfaces;
using Common.Domain.Models;

namespace Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
public sealed class YearlyCumulativeDistanceAchievement : Achievement
{
    private static readonly List<int> s_thresholds = new()
    {
        1500, 2000, 2500, 3000, 4000,
        5000, 6000, 7000, 8000, 9000,
        10000, 15000, 20000
    };

    public YearlyCumulativeDistanceAchievement(long stravaUserId)
        : base(stravaUserId, AchievementType.YearlyCumulativeDistance) { }

    public override void UpdateLevel(IEnumerable<Activity> activities, IDateProvider dateProvider)
    {
        var level = 0;
        if (activities.Any())
        {
            var maxYearDistance = activities
                .GroupBy(activity => activity.StartDateLocal.ToString("yyyy"))
                .Max(group => group
                    .Sum(activity => activity.Distance));

            level = s_thresholds
                .Where(threshold => threshold <= maxYearDistance)
                .Count();
        }

        SetLevel(level, dateProvider);
    }

    public override IEnumerable<object> GetThresholds()
    {
        return s_thresholds
            .Select(e => (object)e)
            .ToList();
    }
}
