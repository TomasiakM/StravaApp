using Common.Domain.DDD;

namespace Athletes.Domain.Aggregates.Athletes.ValueObjects;
public sealed class AthleteId : ValueObject
{
    public Guid Value { get; init; }

    private AthleteId(Guid value) => Value = value;

    public static AthleteId Create() => new(Guid.NewGuid());
    public static AthleteId Create(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
