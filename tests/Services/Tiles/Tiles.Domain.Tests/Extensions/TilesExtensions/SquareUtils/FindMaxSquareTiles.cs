using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.SquareUtils;
public class FindMaxSquareTiles
{
    [Fact]
    public void Should_Return_1_Tile_When_Max_Square_Is_1()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(1, 1, 14)
        };

        var tiles2 = new List<Tile>()
        {
            Tile.Create(0, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 1, 14)
        };

        Assert.Single(tiles.First().FindMaxSquareTiles(tiles, new HashSet<Tile>()));
        Assert.Single(tiles2.First().FindMaxSquareTiles(tiles2, new HashSet<Tile>()));
    }

    [Fact]
    public void Should_Return_4_Tiles_When_Max_Square_Is_2()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(0, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 0, 14),
            Tile.Create(1, 1, 14),
            Tile.Create(0, 2, 14),
            Tile.Create(1, 2, 14)
        };

        var tiles2 = new List<Tile>()
        {
            Tile.Create(0, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 0, 14),
            Tile.Create(1, 1, 14),
            Tile.Create(4, 4, 14),
            Tile.Create(4, 5, 14),
            Tile.Create(5, 4, 14),
            Tile.Create(5, 5, 14)
        };

        Assert.Equal(4, tiles.First().FindMaxSquareTiles(tiles, new HashSet<Tile>()).Count);
        Assert.Equal(4, tiles2.First().FindMaxSquareTiles(tiles2, new HashSet<Tile>()).Count);
    }
}
