using Common.Domain.DDD;

namespace Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
public sealed class ActivityTilesId : ValueObject
{
    public Guid Value { get; init; }

    private ActivityTilesId(Guid value) => Value = value;

    public static ActivityTilesId Create() => new(Guid.NewGuid());
    public static ActivityTilesId Create(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
