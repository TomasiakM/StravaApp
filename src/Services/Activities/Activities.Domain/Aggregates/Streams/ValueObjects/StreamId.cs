using Common.Domain.DDD;

namespace Activities.Domain.Aggregates.Streams.ValueObjects;
public sealed class StreamId : ValueObject
{
    public Guid Value { get; init; }

    private StreamId(Guid value) => Value = value;

    public static StreamId Create() => new(Guid.NewGuid());
    public static StreamId Create(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
