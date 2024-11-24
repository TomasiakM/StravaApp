using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Tests.Aggregates.ActivityTiles.ValueObjects.ActivityTilesIds;
public class Create
{
    [Fact]
    public void ShouldCreateActivityTilesId()
    {
        var activityTilesId = ActivityTilesId.Create();

        Assert.NotEqual(Guid.Empty, activityTilesId.Value);
    }

    [Fact]
    public void ShouldCreateActivityTilesId2()
    {
        var guid = Guid.NewGuid();
        var activityTilesId = ActivityTilesId.Create(guid);

        Assert.Equal(guid, activityTilesId.Value);
    }

    [Fact]
    public void Should2ActivityTilesIdsBeDifferents()
    {
        var activityTilesId = ActivityTilesId.Create();
        var activityTilesId2 = ActivityTilesId.Create();

        Assert.NotEqual(activityTilesId, activityTilesId2);
    }
}
