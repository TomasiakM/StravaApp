using Common.Domain.DDD;

namespace Activities.Domain.Aggregates.Activities.ValueObjects;
public sealed class Watts : ValueObject
{
    public bool DeviceWatts { get; init; }
    public int MaxWatts { get; init; }
    public float AverageWatts { get; init; }

    private Watts(bool deviceWatts, int maxWatts, float averageWatts)
    {
        DeviceWatts = deviceWatts;
        MaxWatts = maxWatts;
        AverageWatts = averageWatts;
    }

    public static Watts Create(bool deviceWatts, int maxWatts, float averageWatts)
        => new(deviceWatts, maxWatts, averageWatts);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return DeviceWatts;
        yield return MaxWatts;
        yield return AverageWatts;
    }
}
