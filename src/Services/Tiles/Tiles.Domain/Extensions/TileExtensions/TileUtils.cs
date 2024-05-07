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
}
