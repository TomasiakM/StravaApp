using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.TileUtils;
public class ToNewClusterTiles
{
    [Fact]
    public void ShouldReturnNewClusterTilesList()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(1, 2),
            Tile.Create(3, 4),
            Tile.Create(5, 6),
        };

        var newClusterTiles = tiles.ToNewClusterTiles();

        Assert.Equal(tiles.Count, newClusterTiles.Count);
        Assert.True(newClusterTiles.Contains(NewClusterTile.Create(1, 2)));
        Assert.True(newClusterTiles.Contains(NewClusterTile.Create(3, 4)));
        Assert.True(newClusterTiles.Contains(NewClusterTile.Create(5, 6)));
    }
}
