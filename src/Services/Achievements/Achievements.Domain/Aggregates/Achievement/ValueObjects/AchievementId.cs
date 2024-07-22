using Common.Domain.DDD;

namespace Achievements.Domain.Aggregates.Achievement.ValueObjects;
public sealed class AchievementId : ValueObject
{
    public Guid Value { get; init; }

    private AchievementId(Guid value) => Value = value;

    public static AchievementId Create() => new(Guid.NewGuid());
    public static AchievementId Create(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
