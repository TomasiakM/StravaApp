using Common.Domain.DDD;

namespace Activities.Domain.Aggregates.Activities.ValueObjects;
public sealed class Speed : ValueObject
{
    public float MaxSpeed { get; init; }
    public float AverageSpeed { get; init; }

    private Speed(float maxSpeed, float averageSpeed)
    {
        MaxSpeed = maxSpeed;
        AverageSpeed = averageSpeed;
    }

    public static Speed Create(float maxSpeed, float averageSpeed)
        => new(maxSpeed, averageSpeed);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MaxSpeed;
        yield return AverageSpeed;
    }
}
