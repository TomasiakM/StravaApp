using Common.Domain.DDD;

namespace Strava.Domain.Aggregates.Token.ValueObjects;
public sealed class TokenId : ValueObject
{
    public Guid Value { get; init; }

    private TokenId(Guid value) => Value = value;

    public static TokenId Create() => new(Guid.NewGuid());
    public static TokenId Create(Guid value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
