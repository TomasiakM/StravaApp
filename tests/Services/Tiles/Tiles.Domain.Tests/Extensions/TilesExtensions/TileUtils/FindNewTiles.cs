using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.TileUtils;
public class FindNewTiles
{
    [Fact]
    public void Should_Return_Collection_With_Single_Tile()
    {
        var tiles = new List<Tile>();
        var newTiles = new List<Tile>
        {
            Tile.Create(1, 1)
        };

        Assert.Single(tiles.FindNewTiles(newTiles));
    }

    [Fact]
    public void Should_Return_Empty_Collection()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(0, 1),
            Tile.Create(1, 0),
            Tile.Create(1, 1)
        };
        var newTiles = new List<Tile>
        {
            Tile.Create(0, 1),
            Tile.Create(1, 1),
        };

        Assert.Empty(tiles.FindNewTiles(newTiles));
    }

    [Fact]
    public void Should_Return_Collection_With_3_Tiles()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(0, 1),
            Tile.Create(1, 0),
            Tile.Create(1, 1)
        };
        var newTiles = new List<Tile>
        {
            Tile.Create(0, 1),
            Tile.Create(1, 0),
            Tile.Create(2, 2),
            Tile.Create(1, 2),
        };

        Assert.Equal(2, tiles.FindNewTiles(newTiles).Count);
    }
}
