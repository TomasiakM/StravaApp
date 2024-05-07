using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.ClusterUtils;
public class IsCluster
{
    [Fact]
    public void Should_Return_True_When_Tile_Is_Cluster()
    {
        var tile = Tile.Create(1, 1, 14);

        var tiles = new List<Tile>
        {
            Tile.Create(1, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(2, 1, 14),
            Tile.Create(1, 2, 14),
        };

        // 0, 1, 0
        // 1, X, 1
        // 0, 1, 0

        Assert.True(tile.IsCluster(tiles));
    }

    [Fact]
    public void Should_Return_False_When_Tile_Is_Not_Cluster()
    {
        var tile = Tile.Create(1, 1, 14);

        var tiles = new List<Tile>
        {
            Tile.Create(0, 0, 14),
            Tile.Create(2, 0, 14),
            Tile.Create(0, 2, 14),
            Tile.Create(2, 2, 14),
        };

        // 1, 0, 1
        // 0, X, 0
        // 1, 0, 1

        Assert.False(tile.IsCluster(tiles));
    }
}
