using Common.Domain.DDD;

namespace Activities.Domain.Aggregates.Activities.ValueObjects;
public sealed class ActivityId : ValueObject
{
    public Guid Value { get; set; }

    private ActivityId(Guid value) => Value = value;

    public static ActivityId Create() => new(Guid.NewGuid());
    public static ActivityId Create(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
