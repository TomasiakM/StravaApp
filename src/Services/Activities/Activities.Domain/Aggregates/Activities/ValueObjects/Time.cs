using Common.Domain.DDD;

namespace Activities.Domain.Aggregates.Activities.ValueObjects;
public sealed class Time : ValueObject
{
    public int MovingTime { get; init; }
    public int ElapsedTime { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime StartDateLocal { get; init; }

    private Time(int movingTime, int elapseTime, DateTime startDate, DateTime startDateLocal)
    {
        MovingTime = movingTime;
        ElapsedTime = elapseTime;
        StartDate = startDate;
        StartDateLocal = startDateLocal;
    }

    public static Time Create(int movingTime, int elapseTime, DateTime startDate, DateTime startDateLocal)
        => new(movingTime, elapseTime, startDate, startDateLocal);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MovingTime;
        yield return ElapsedTime;
        yield return StartDate;
        yield return StartDateLocal;
    }

    private Time() { }
}
