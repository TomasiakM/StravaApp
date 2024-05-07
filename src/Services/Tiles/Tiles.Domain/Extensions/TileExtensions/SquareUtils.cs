using Common.Domain.Extensions;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Extensions.TileExtensions;
public static class SquareUtils
{
    public static ICollection<Tile> FindNewSquareTiles(this IEnumerable<Tile> tiles, IEnumerable<Tile> activityTiles)
    {
        var tilesSet = tiles.ToHashSet();
        var processedTilesSet = new HashSet<Tile>();

        var squares = new List<ICollection<Tile>>();
        foreach (var tile in tilesSet)
        {
            if (processedTilesSet.Contains(tile)) continue;
            var sqare = tile.FindMaxSquareTiles(tilesSet, processedTilesSet);

            if (squares.Any() && sqare.Count > squares.First().Count)
            {
                squares.Clear();
                squares.Add(sqare);
                continue;
            }

            squares.Add(sqare);
        }

        tilesSet.AddRange(activityTiles);

        var newSquare = new HashSet<Tile>();
        processedTilesSet.Clear();

        foreach (var tile in tilesSet)
        {
            if (processedTilesSet.Contains(tile)) continue;

            var square = tile.FindMaxSquareTiles(tilesSet, processedTilesSet);
            if (square.Count > newSquare.Count)
            {
                newSquare = square.ToHashSet();
            }
        }


        var squareToComparison = squares.FirstOrDefault(e => !e.Except(newSquare).Any());
        if (squareToComparison is null)
        {
            return newSquare;
        }

        return newSquare
            .Except(squareToComparison)
            .ToHashSet();
    }

    public static ICollection<Tile> FindMaxSquareTiles(this Tile tile, IEnumerable<Tile> allTiles, ICollection<Tile> processedTilesSet)
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

                processedTilesSet.Add(right);
                processedTilesSet.Add(bottom);

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
        var processedTilesSet = new HashSet<Tile>();
        var maxSquare = 0;

        foreach (var tile in tiles)
        {
            var maxSquareTiles = tile.FindMaxSquareTiles(tiles, processedTilesSet);

            var size = Math.Sqrt(maxSquareTiles.Count);
            if (size > maxSquare)
            {
                maxSquare = (int)size;
            }
        }

        return maxSquare;
    }
}
