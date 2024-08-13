using Activities.Domain.Aggregates.Activities.ValueObjects;

namespace Activities.Domain.Tests.Aggregates.Activities.ValueObjects.Watt;
public class Create
{
    [Fact]
    public void ShouldCreateWatts()
    {
        var deviceWatts = true;
        var maxWatts = 222;
        var averageWatts = 134.6f;

        var watts = Watts.Create(deviceWatts, maxWatts, averageWatts);

        Assert.Equal(deviceWatts, watts.DeviceWatts);
        Assert.Equal(maxWatts, watts.MaxWatts);
        Assert.Equal(averageWatts, watts.AverageWatts);
    }
}
