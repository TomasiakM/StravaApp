using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Extensions.TileExtensions;
public static class TileUtils
{
    public static ICollection<Tile> FindNewTiles(this IEnumerable<Tile> tiles, IEnumerable<Tile> activityTiles)
    {
        return activityTiles
            .Except(tiles)
            .ToHashSet();
    }

    public static ICollection<NewTile> ToNewTiles(this IEnumerable<Tile> tiles)
    {
        return tiles
            .Select(e => NewTile.Create(e.X, e.Y, e.Z))
            .ToHashSet();
    }

    public static ICollection<NewClusterTile> ToNewClusterTiles(this IEnumerable<Tile> tiles)
    {
        return tiles
            .Select(e => NewClusterTile.Create(e.X, e.Y, e.Z))
            .ToHashSet();
    }
}
