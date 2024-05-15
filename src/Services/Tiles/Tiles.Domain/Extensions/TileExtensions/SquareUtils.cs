using Common.Domain.Extensions;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Extensions.TileExtensions;
public static class SquareUtils
{
    public static ICollection<Tile> FindMaxSquareTiles(this Tile tile, IEnumerable<Tile> allTiles)
    {
        var size = 1;
        var newSizeAvailable = true;

        var squareTiles = new HashSet<Tile>() { tile };
        while (newSizeAvailable)
        {
            var newSizeTiles = new List<Tile>();
            for (var i = 0; i <= size; i++)
            {
                var right = Tile.Create(tile.X + size, tile.Y + i, tile.Z);
                var bottom = Tile.Create(tile.X + i, tile.Y + size, tile.Z);

                if (allTiles.Contains(right) && allTiles.Contains(bottom))
                {
                    newSizeTiles.Add(right);
                    newSizeTiles.Add(bottom);

                    continue;
                }

                newSizeAvailable = false;
                break;
            }

            if (newSizeAvailable)
            {
                squareTiles.AddRange(newSizeTiles);
            }

            size++;
        }

        return squareTiles;
    }

    public static int MaxSquare(this IEnumerable<Tile> tilesSet)
    {
        var tiles = tilesSet.ToHashSet();
        var maxSquare = 0;

        foreach (var tile in tiles)
        {
            var maxSquareTiles = tile.FindMaxSquareTiles(tiles);

            var size = Math.Sqrt(maxSquareTiles.Count);
            if (size > maxSquare)
            {
                maxSquare = (int)size;
            }
        }

        return maxSquare;
    }
}
