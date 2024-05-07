namespace Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
public sealed class NewSquareTile : Tile
{
    private NewSquareTile(int x, int y, int z)
        : base(x, y, z) { }

    public new static NewSquareTile Create(int x, int y, int z) => new(x, y, z);
}
