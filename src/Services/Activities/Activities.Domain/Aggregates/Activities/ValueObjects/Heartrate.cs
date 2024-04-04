using Common.Domain.DDD;

namespace Activities.Domain.Aggregates.Activities.ValueObjects;
public sealed class Heartrate : ValueObject
{
    public bool HasHeartrate { get; init; }
    public float MaxHeartrate { get; init; }
    public float AverageHeartrate { get; init; }

    private Heartrate(bool hasHeartrate, float maxHeartrate, float averageHeartrate)
    {
        HasHeartrate = hasHeartrate;
        MaxHeartrate = maxHeartrate;
        AverageHeartrate = averageHeartrate;
    }

    public static Heartrate Create(bool hasHeartrate, float maxHeartrate, float averageHeartrate)
        => new(hasHeartrate, maxHeartrate, averageHeartrate);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return HasHeartrate;
        yield return MaxHeartrate;
        yield return AverageHeartrate;
    }
}
