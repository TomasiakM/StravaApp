using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Tests.Aggregates.ActivityTiles.ValueObjects.NewClusters;
public class Create
{
    [Fact]
    public void ShouldCreateNewCluster()
    {
        var x = 3;
        var y = 66;

        var newClusterTile = NewClusterTile.Create(x, y);

        Assert.Equal(x, newClusterTile.X);
        Assert.Equal(y, newClusterTile.Y);
        Assert.Equal(Tile.DEFAULT_TILE_ZOOM, newClusterTile.Z);
    }
}
