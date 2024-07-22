using Common.Domain.DDD;

namespace Achievements.Domain.Aggregates.Achievement.Entities;
public sealed class AchievementLevel : Entity<int>
{
    public int Level { get; init; }
    public DateTimeOffset AchievedAt { get; init; }

    private AchievementLevel(int level, DateTimeOffset achievedAt)
        : base(0)
    {
        Level = level;
        AchievedAt = achievedAt;
    }

    public static AchievementLevel Create(int level, DateTimeOffset achivedAt)
        => new(level, achivedAt);

    private AchievementLevel() : base(0) { }
}
