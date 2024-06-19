using Common.Domain.DDD;

namespace Tiles.Domain.Aggregates.Coordinates.ValueObjects;
public sealed class CoordinatesId : ValueObject
{
    public Guid Value { get; init; }

    private CoordinatesId(Guid value) => Value = value;

    public static CoordinatesId Create() => new(Guid.NewGuid());
    public static CoordinatesId Create(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
