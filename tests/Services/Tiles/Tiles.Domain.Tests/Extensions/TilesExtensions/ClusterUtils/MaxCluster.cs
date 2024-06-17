using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.ClusterUtils;
public class MaxCluster
{
    [Fact]
    public void Should_Return_0_When_List_With_Tiles_NotContaining_Clusters()
    {
        var tiles = new List<Tile>
        {
            Tile.Create(1, 0),
            Tile.Create(0, 1),
            Tile.Create(2, 1),
            Tile.Create(1, 2),
        };

        Assert.Equal(0, tiles.MaxCluster());
    }

    [Fact]
    public void Should_Return_1_When_List_With_Tiles_Containing_Single_Cluster()
    {
        var tiles = new List<Tile>
        {
            Tile.Create(1, 0),
            Tile.Create(0, 1),
            Tile.Create(1, 1),
            Tile.Create(2, 1),
            Tile.Create(1, 2),
        };

        Assert.Equal(1, tiles.MaxCluster());
    }

    [Fact]
    public void Should_Return_1_When_List_With_Tiles_Containing_Many_Single_Clusters()
    {
        var tiles = new List<Tile>
        {
            Tile.Create(1, 0),
            Tile.Create(0, 1),
            Tile.Create(1, 1),
            Tile.Create(2, 1),
            Tile.Create(1, 2),

            Tile.Create(4, 3),
            Tile.Create(3, 4),
            Tile.Create(4, 4),
            Tile.Create(5, 4),
            Tile.Create(4, 5),
        };

        Assert.Equal(1, tiles.MaxCluster());
    }

    [Fact]
    public void Sould_Return_2_When_List_Containing_2_Clusters_Next_To_Each_Other()
    {
        var tiles = new List<Tile>
        {
            Tile.Create(1, 0),
            Tile.Create(2, 0),
            Tile.Create(0, 1),
            Tile.Create(1, 1),
            Tile.Create(2, 1),
            Tile.Create(3, 1),
            Tile.Create(1, 2),
            Tile.Create(2, 2),
        };

        Assert.Equal(2, tiles.MaxCluster());
    }
}
