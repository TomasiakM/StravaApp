using Activities.Domain.Aggregates.Activities.ValueObjects;

namespace Activities.Domain.Tests.Aggregates.Activities.ValueObjects.Heartrates;
public class Create
{
    [Fact]
    public void ShouldCreateHeartrate()
    {
        var hasHeartrata = true;
        var maxHeartrate = 145.2f;
        var averageHeartrate = 120.5f;

        var heartrate = Heartrate.Create(hasHeartrata, maxHeartrate, averageHeartrate);

        Assert.Equal(hasHeartrata, heartrate.HasHeartrate);
        Assert.Equal(maxHeartrate, heartrate.MaxHeartrate);
        Assert.Equal(averageHeartrate, heartrate.AverageHeartrate);
    }
}
