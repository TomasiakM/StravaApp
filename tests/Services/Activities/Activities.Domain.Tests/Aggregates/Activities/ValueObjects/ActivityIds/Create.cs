using Activities.Domain.Aggregates.Activities.ValueObjects;

namespace Activities.Domain.Tests.Aggregates.Activities.ValueObjects.ActivityIds;
public class Create
{
    [Fact]
    public void ShouldCreateActivityId()
    {
        var activityId = ActivityId.Create();

        Assert.NotEqual(Guid.Empty, activityId.Value);
    }

    [Fact]
    public void ShouldCreateActivityId2()
    {
        var guid = Guid.NewGuid();
        var activityId = ActivityId.Create(guid);

        Assert.Equal(guid, activityId.Value);
    }

    [Fact]
    public void Should2ActivityIdsBeDifferents()
    {
        var activityId = ActivityId.Create();
        var activityId2 = ActivityId.Create();

        Assert.NotEqual(activityId.Value, activityId2.Value);
    }
}
