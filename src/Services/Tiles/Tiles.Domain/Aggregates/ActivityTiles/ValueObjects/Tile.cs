using Common.Domain.DDD;

namespace Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
public class Tile : ValueObject
{
    public const int DEFAULT_TILE_ZOOM = 14;

    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }

    protected Tile(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Tile Create(int x, int y, int z = DEFAULT_TILE_ZOOM) =>
        new(x, y, z);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }
}
