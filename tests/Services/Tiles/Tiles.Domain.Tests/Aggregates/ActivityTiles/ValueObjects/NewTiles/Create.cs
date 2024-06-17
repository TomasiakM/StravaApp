using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Tests.Aggregates.ActivityTiles.ValueObjects.NewTiles;
public class Create
{
    [Fact]
    public void ShouldCreateNewTile()
    {
        var x = 6;
        var y = 9;

        var newTile = NewTile.Create(x, y);

        Assert.Equal(x, newTile.X);
        Assert.Equal(y, newTile.Y);
        Assert.Equal(Tile.DEFAULT_TILE_ZOOM, newTile.Z);
    }
}
