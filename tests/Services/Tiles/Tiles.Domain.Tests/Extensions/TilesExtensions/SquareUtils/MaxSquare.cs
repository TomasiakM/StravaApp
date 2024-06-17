using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.SquareUtils;
public class MaxSquare
{
    [Fact]
    public void Should_Return_0_When_List_Is_Empty()
    {
        var tiles = new List<Tile>();

        Assert.Equal(0, tiles.MaxSquare());
    }

    [Fact]
    public void Should_Return_1_When_List_Containing_Silgle_Element()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(1, 1)
        };

        Assert.Equal(1, tiles.MaxSquare());
    }

    [Fact]
    public void Should_Return_1_When_Max_Square_Is_1()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(0, 0),
            Tile.Create(0, 1),
            Tile.Create(1, 1),
        };

        Assert.Equal(1, tiles.MaxSquare());
    }

    [Fact]
    public void Should_Return_2_When_Max_Square_Is_2()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(0, 0),
            Tile.Create(0, 1),
            Tile.Create(1, 0),
            Tile.Create(1, 1),
        };

        Assert.Equal(2, tiles.MaxSquare());
    }
}
