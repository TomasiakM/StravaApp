using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Achievements.Domain.Aggregates.Achievement.Enums;
using Achievements.Domain.Interfaces;

namespace Achievements.Domain.Aggregates.Achievement.Factories;
public sealed class AchievementFactory : IAchievementFactory
{
    private static readonly Dictionary<AchievementType, Type> s_achievementMap = new()
    {
        { AchievementType.MaxDistance, typeof(MaxDistanceAchievement) },
        { AchievementType.CumulativeDistance, typeof(CumulativeDistanceAchievement) },
        { AchievementType.MonthlyCumulativeDistance, typeof(MonthlyCumulativeDistanceAchievement) },
        { AchievementType.YearlyCumulativeDistance, typeof(YearlyCumulativeDistanceAchievement) }
    };

    public Achievement Create(long stravaUserId, AchievementType achievementType)
    {
        if (s_achievementMap.TryGetValue(achievementType, out var achievementTypeClass))
        {
            return (Achievement)Activator.CreateInstance(achievementTypeClass, stravaUserId)!;
        }

        throw new NotImplementedException();
    }

    public ICollection<Achievement> CreateAll(long stravaUserId, IEnumerable<Achievement>? without = null)
    {
        without ??= new List<Achievement>();

        var achievements = new List<Achievement>();
        var withoutTypes = without.Select(a => a.GetType());

        foreach (var entry in s_achievementMap)
        {
            if (!withoutTypes.Any(type => type == entry.Value))
            {
                var achievement = (Achievement)Activator.CreateInstance(entry.Value, stravaUserId)!;
                achievements.Add(achievement);
            }
        }

        return achievements;
    }
}
