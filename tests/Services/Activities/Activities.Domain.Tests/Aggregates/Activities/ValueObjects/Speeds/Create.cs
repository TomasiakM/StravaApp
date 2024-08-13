using Activities.Domain.Aggregates.Activities.ValueObjects;

namespace Activities.Domain.Tests.Aggregates.Activities.ValueObjects.Speeds;
public class Create
{
    [Fact]
    public void ShouldCreateSpeed()
    {
        var maxSpeed = 33.3f;
        var averageSpeed = 29.9f;

        var speed = Speed.Create(maxSpeed, averageSpeed);

        Assert.Equal(maxSpeed, speed.MaxSpeed);
        Assert.Equal(averageSpeed, speed.AverageSpeed);
    }
}
