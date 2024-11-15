using Achievements.Domain.Aggregates.Achievement.Entities;
using Achievements.Domain.Aggregates.Achievement.Enums;
using Achievements.Domain.Aggregates.Achievement.ValueObjects;
using Common.Domain.DDD;
using Common.Domain.Interfaces;
using Common.Domain.Models;

namespace Achievements.Domain.Aggregates.Achievement;
public abstract class Achievement : AggregateRoot<AchievementId>
{
    private List<AchievementLevel> _achievementLevels = new();
    public IReadOnlyList<AchievementLevel> AchievementLevels => _achievementLevels.AsReadOnly();

    public long StravaUserId { get; init; }
    public AchievementType AchievementType { get; init; }

    protected Achievement(long stravaUserId, AchievementType achievementType)
        : base(AchievementId.Create())
    {
        StravaUserId = stravaUserId;
        AchievementType = achievementType;
    }

    protected void SetLevel(int level, IDateProvider dateProvider)
    {
        if (level < 0)
        {
            throw new ArgumentException("Level cannot be negative");
        }

        if (level > GetThresholds().Count())
        {
            throw new ArgumentException("Level cannot be greater than thresholds count");
        }

        var currentMaxLevel = _achievementLevels.Count == 0
            ? 0 :
            _achievementLevels.Max(e => e.Level);
        if (currentMaxLevel > level)
        {
            _achievementLevels = _achievementLevels
                .Where(e => e.Level <= level)
                .ToList();

            return;
        }

        for (int i = currentMaxLevel + 1; i <= level; i++)
        {
            _achievementLevels.Add(AchievementLevel.Create(i, dateProvider.OffsetUtcNow));
        }
    }

    public abstract void UpdateLevel(IEnumerable<Activity> activities, IDateProvider dateProvider);
    public abstract IEnumerable<object> GetThresholds();
}