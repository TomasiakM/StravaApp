using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.ClusterUtils;
public class FindNeighbourTileCluster
{
    [Fact]
    public void Should_Return_Single_Tile()
    {
        var tile = Tile.Create(1, 1);
        var clusters = new List<Tile>
        {
            tile
        };

        var processed = new HashSet<Tile>();


        Assert.Single(tile.FindNeighbourTileCluster(clusters, processed));
    }

    [Fact]
    public void Should_Return_5_When_Max_Cluster_Is_5()
    {
        var tile = Tile.Create(1, 1);
        var clusters = new List<Tile>
        {
            Tile.Create(1, 0),
            Tile.Create(0, 1),
            tile,
            Tile.Create(2, 1),
            Tile.Create(1, 2),

            Tile.Create(4, 4),
        };

        // 0, 1, 0,
        // 1, 1, 1,
        // 0, 1, 0,

        var processed = new HashSet<Tile>();

        Assert.Equal(5, tile.FindNeighbourTileCluster(clusters, processed).Count);
    }
}
