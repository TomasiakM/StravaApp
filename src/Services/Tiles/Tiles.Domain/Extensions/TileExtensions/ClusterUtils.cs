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

        tilesSet.AddRange(activityTiles);

        var allNewClusters = tilesSet
            .Where(e => e.IsCluster(tilesSet))
            .ToHashSet();

        return allNewClusters
            .Except(allClusters)
            .ToList();
    }

    public static bool IsCluster(this Tile tile, IEnumerable<Tile> tiles)
    {
        var left = Tile.Create(tile.X - 1, tile.Y, tile.Z);
        var top = Tile.Create(tile.X, tile.Y + 1, tile.Z);
        var right = Tile.Create(tile.X + 1, tile.Y, tile.Z);
        var bottom = Tile.Create(tile.X, tile.Y - 1, tile.Z);

        return tiles.Contains(left) &&
            tiles.Contains(top) &&
            tiles.Contains(right) &&
            tiles.Contains(bottom);
    }

    public static int MaxCluster(this IEnumerable<Tile> tiles)
    {
        var maxCluster = 0;
        var clustersSet = tiles
            .Where(e => e.IsCluster(tiles))
            .ToHashSet();

        var processedTilesSet = new HashSet<Tile>();

        foreach (var tile in clustersSet)
        {
            if (processedTilesSet.Contains(tile)) continue;

            var foundedCluster = tile.FindNeighbourTileCluster(clustersSet, processedTilesSet);
            if (foundedCluster.Count > maxCluster)
            {
                maxCluster = foundedCluster.Count;
            }
        }

        return maxCluster;
    }

    public static ICollection<Tile> FindNeighbourTileCluster(this Tile tile, IEnumerable<Tile> clusters, ICollection<Tile> processedTiles)
    {
        processedTiles.Add(tile);
        var cluster = new HashSet<Tile>() { tile };

        var left = Tile.Create(tile.X - 1, tile.Y, tile.Z);
        var top = Tile.Create(tile.X, tile.Y + 1, tile.Z);
        var right = Tile.Create(tile.X + 1, tile.Y, tile.Z);
        var bottom = Tile.Create(tile.X, tile.Y - 1, tile.Z);

        if (!processedTiles.Contains(left) && clusters.Contains(left))
        {
            processedTiles.Add(left);
            cluster.AddRange(left.FindNeighbourTileCluster(clusters, processedTiles));
        }

        if (!processedTiles.Contains(top) && clusters.Contains(top))
        {
            processedTiles.Add(top);
            cluster.AddRange(top.FindNeighbourTileCluster(clusters, processedTiles));
        }

        if (!processedTiles.Contains(right) && clusters.Contains(right))
        {
            processedTiles.Add(right);
            cluster.AddRange(right.FindNeighbourTileCluster(clusters, processedTiles));
        }

        if (!processedTiles.Contains(bottom) && clusters.Contains(bottom))
        {
            processedTiles.Add(bottom);
            cluster.AddRange(bottom.FindNeighbourTileCluster(clusters, processedTiles));
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
