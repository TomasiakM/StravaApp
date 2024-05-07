using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.SquareUtils;
public class FindNewSquareTiles
{
    [Fact]
    public void Should_Return_0_Tiles_When_Lists_Are_Empty()
    {
        var tiles = new List<Tile>();
        var newTiles = new List<Tile>();

        Assert.Equal(0, tiles.FindNewSquareTiles(newTiles).Count);
    }

    [Fact]
    public void Should_Return_1_Tile_When_Max_Square_Increase_From_0_To_1()
    {
        var tiles = new List<Tile>();

        var newTiles = new List<Tile>()
        {
            Tile.Create(0, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 1, 14)
        };

        Assert.Equal(1, tiles.FindNewSquareTiles(newTiles).Count);
    }

    [Fact]
    public void Should_Return_4_Tiles_When_Max_Square_Increase_From_0_To_2()
    {
        var tiles = new List<Tile>();

        var newTiles = new List<Tile>()
        {
            Tile.Create(0, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 0, 14),
            Tile.Create(1, 1, 14)
        };

        Assert.Equal(4, tiles.FindNewSquareTiles(newTiles).Count);
    }

    [Fact]
    public void Should_Return_5_Tiles_When_Max_Square_Increase_From_2_To_3()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(0, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 0, 14),
            Tile.Create(1, 1, 14)
        };

        var newTiles = new List<Tile>()
        {
            Tile.Create(0, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 0, 14),
            Tile.Create(1, 1, 14),
            Tile.Create(2, 0, 14),
            Tile.Create(2, 1, 14),
            Tile.Create(2, 2, 14),
            Tile.Create(0, 2, 14),
            Tile.Create(1, 2, 14),
            Tile.Create(0, 3, 14),
            Tile.Create(1, 3, 14),
        };

        Assert.Equal(5, tiles.FindNewSquareTiles(newTiles).Count);
    }
}
