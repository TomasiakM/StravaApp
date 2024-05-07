namespace Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
public sealed class NewClusterTile : Tile
{
    private NewClusterTile(int x, int y, int z)
        : base(x, y, z) { }

    public new static NewClusterTile Create(int x, int y, int z) => new(x, y, z);
}
