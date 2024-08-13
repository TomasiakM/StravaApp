using Activities.Domain.Aggregates.Activities.ValueObjects;

namespace Activities.Domain.Tests.Aggregates.Activities.ValueObjects.Times;
public class Create
{
    [Fact]
    public void ShouldCreateTime()
    {
        var movingTime = 101;
        var elapseTime = 105;
        var startDate = new DateTime(2022, 1, 1, 10, 10, 10);
        var startDateLocal = new DateTime(2022, 1, 1, 11, 10, 10);

        var time = Time.Create(movingTime, elapseTime, startDate, startDateLocal);

        Assert.Equal(movingTime, time.MovingTime);
        Assert.Equal(elapseTime, time.ElapsedTime);
        Assert.Equal(startDate, time.StartDate);
        Assert.Equal(startDateLocal, time.StartDateLocal);
    }

}
