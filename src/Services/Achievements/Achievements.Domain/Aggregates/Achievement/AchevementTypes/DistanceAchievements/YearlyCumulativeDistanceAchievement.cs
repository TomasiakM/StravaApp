using Achievements.Domain.Aggregates.Achievement.Enums;
using Common.Domain.Interfaces;
using Common.Domain.Models;

namespace Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
public sealed class YearlyCumulativeDistanceAchievement : Achievement
{
    private static readonly List<int> s_thresholds = new()
    {
        100, 200, 300, 400, 500,
        600, 700, 800, 900, 1000,
        1250, 1500, 1750, 2000
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
