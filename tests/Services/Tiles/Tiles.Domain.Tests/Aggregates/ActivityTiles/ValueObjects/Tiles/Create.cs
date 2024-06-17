using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Tests.Aggregates.ActivityTiles.ValueObjects.Tiles;
public class Create
{
    [Fact]
    public void ShouldCreateTile()
    {
        var x = 6;
        var y = 9;

        var tile = Tile.Create(x, y);

        Assert.Equal(x, tile.X);
        Assert.Equal(y, tile.Y);
        Assert.Equal(Tile.DEFAULT_TILE_ZOOM, tile.Z);
    }
}
