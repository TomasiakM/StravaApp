using Achievements.Domain.Aggregates.Achievement.ValueObjects;
using Common.Domain.Interfaces;

namespace Achievements.Domain.Aggregates.Achievement;
public interface IAchievementRepository : IRepository<Achievement, AchievementId>
{
}
