using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.TileUtils;
public class ToNewTiles
{
    [Fact]
    public void ShouldReturnNewTilesList()
    {
        var tiles = new List<Tile>()
        {
            Tile.Create(1, 2),
            Tile.Create(3, 4),
            Tile.Create(5, 6),
        };

        var newTiles = tiles.ToNewTiles();

        Assert.Equal(tiles.Count, newTiles.Count);
        Assert.True(newTiles.Contains(NewTile.Create(1, 2)));
        Assert.True(newTiles.Contains(NewTile.Create(3, 4)));
        Assert.True(newTiles.Contains(NewTile.Create(5, 6)));
    }
}
