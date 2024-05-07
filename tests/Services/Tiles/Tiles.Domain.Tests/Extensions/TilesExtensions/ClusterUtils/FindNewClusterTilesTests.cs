﻿using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Tests.Extensions.TilesExtensions.ClusterUtils;
public class FindNewClusterTilesTests
{
    [Fact]
    public void Should_Find_New_Clusters_Tiles()
    {
        var tiles = new List<Tile>();

        var newTiles = new List<Tile>
        {
            Tile.Create(1, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 1, 14),
            Tile.Create(2, 1, 14),
            Tile.Create(1, 2, 14),
        };

        var newClusterTiles = tiles.FindNewClusterTiles(newTiles);

        Assert.Single(newClusterTiles);
        Assert.Equal(Tile.Create(1, 1, 14), newClusterTiles.First());
    }

    [Fact]
    public void Should_Find_NotFind_Any_Clusters_Tiles()
    {
        var tiles = new List<Tile>() {
            Tile.Create(1, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 1, 14),
            Tile.Create(2, 1, 14),
            Tile.Create(1, 2, 14),
        };

        var newTiles = new List<Tile>
        {
            Tile.Create(1, 0, 14),
            Tile.Create(0, 1, 14),
            Tile.Create(1, 1, 14),
            Tile.Create(2, 1, 14),
            Tile.Create(1, 2, 14),
            Tile.Create(2, 2, 14),
        };

        var newClusterTiles = tiles.FindNewClusterTiles(newTiles);

        Assert.Empty(newClusterTiles);
    }
}
