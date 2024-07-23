using Achievements.Domain.Aggregates.Achievement;
using Achievements.Domain.Aggregates.Achievement.Enums;

namespace Achievements.Domain.Interfaces;
public interface IAchievementFactory
{
    Achievement Create(long stravaUserId, AchievementType achievementType);
    ICollection<Achievement> CreateAll(long stravaUserId, IEnumerable<Achievement>? without);
}
