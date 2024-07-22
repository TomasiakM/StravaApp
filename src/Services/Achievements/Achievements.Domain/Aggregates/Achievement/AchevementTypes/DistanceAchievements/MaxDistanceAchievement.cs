using Achievements.Domain.Aggregates.Achievement.Enums;
using Common.Domain.Interfaces;
using Common.Domain.Models;

namespace Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
public sealed class MaxDistanceAchievement : Achievement
{
    private static readonly List<int> s_thresholds = new()
    {
        25, 50, 75, 100,
        150, 200, 250,
        300, 400, 500,
    };

    public MaxDistanceAchievement(long stravaUserId)
        : base(stravaUserId, AchievementType.MaxDistance) { }

    public override void UpdateLevel(IEnumerable<Activity> activities, IDateProvider dateProvider)
    {
        var level = 0;
        if (activities.Any())
        {
            var maxDistance = activities
                .Max(activity => activity.Distance);

            level = s_thresholds
                .Where(threshold => threshold <= maxDistance)
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
