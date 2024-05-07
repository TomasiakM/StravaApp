using Common.Domain.Extensions;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Extensions.TileExtensions;
public static class ClusterUtils
{
    public static ICollection<Tile> FindNewClusterTiles(this IEnumerable<Tile> tiles, IEnumerable<Tile> activityTiles)
    {
        var tilesSet = tiles.ToHashSet();

        var allClusters = tilesSet
            .Where(e => e.IsCluster(tilesSet))
            .ToHashSet();

        foreach (var tile in activityTiles)
        {
            tilesSet.Add(tile);
        }

        return tilesSet
            .Where(tile => tile.IsCluster(tilesSet))
            .Except(allClusters)
            .ToList();
    }

    public static bool IsCluster(this Tile tile, IEnumerable<Tile> tiles)
    {
        var left = Tile.Create(tile.X - 1, tile.Z, tile.Y);
        var top = Tile.Create(tile.X, tile.Z + 1, tile.Y);
        var right = Tile.Create(tile.X + 1, tile.Z, tile.Y);
        var bottom = Tile.Create(tile.X, tile.Z - 1, tile.Y);

        return tiles.Contains(left) &&
            tiles.Contains(top) &&
            tiles.Contains(right) &&
            tiles.Contains(bottom);
    }

    public static int MaxCluster(this IEnumerable<Tile> tiles)
    {
        var maxCluster = 0;
        var tilesSet = tiles
            .Where(e => e.IsCluster(tiles))
            .ToHashSet();
        var processedTilesSet = new HashSet<Tile>();

        foreach (var tile in tilesSet)
        {
            if (processedTilesSet.Contains(tile)) continue;

            var foundedCluster = tile.FindNeighbourTileCluster(tilesSet, processedTilesSet);
            if (foundedCluster.Count > maxCluster)
            {
                maxCluster = foundedCluster.Count;
            }
        }

        return maxCluster;
    }

    public static ICollection<Tile> FindNeighbourTileCluster(this Tile tile, IEnumerable<Tile> tiles, ICollection<Tile> processedTiles)
    {
        processedTiles.Add(tile);

        if (!tile.IsCluster(tiles))
        {
            return new HashSet<Tile>();
        }

        var left = Tile.Create(tile.X - 1, tile.Z, tile.Y);
        var top = Tile.Create(tile.X, tile.Z + 1, tile.Y);
        var right = Tile.Create(tile.X + 1, tile.Z, tile.Y);
        var bottom = Tile.Create(tile.X, tile.Z - 1, tile.Y);

        processedTiles.Add(tile);
        var cluster = new HashSet<Tile>() { tile };
        if (!processedTiles.Contains(left) && tiles.Contains(left))
        {
            processedTiles.Add(left);
            cluster.AddRange(left.FindNeighbourTileCluster(tiles, processedTiles));
        }

        if (!processedTiles.Contains(top) && tiles.Contains(top))
        {
            processedTiles.Add(top);
            cluster.AddRange(top.FindNeighbourTileCluster(tiles, processedTiles));
        }

        if (!processedTiles.Contains(right) && tiles.Contains(right))
        {
            processedTiles.Add(right);
            cluster.AddRange(right.FindNeighbourTileCluster(tiles, processedTiles));
        }

        if (!processedTiles.Contains(bottom) && tiles.Contains(bottom))
        {
            processedTiles.Add(bottom);
            cluster.AddRange(bottom.FindNeighbourTileCluster(tiles, processedTiles));
        }

        return cluster;
    }

    public static int AllClusters(this IEnumerable<Tile> tiles)
    {
        var tilesSet = tiles.ToHashSet();

        return tiles
            .ToHashSet()
            .Where(tile => tile.IsCluster(tilesSet))
            .Count();
    }
}
