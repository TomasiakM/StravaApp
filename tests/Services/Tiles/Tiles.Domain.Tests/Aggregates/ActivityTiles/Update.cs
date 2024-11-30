using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Tests.Aggregates.ActivityTiles;
public class Update
{
    [Fact]
    public void ShouldUpdateActivityTileAggregate()
    {
        var prevTiles = new List<Tile>()
        {
            Tile.Create(0, 0),
                               Tile.Create(1, 1),
        };
        var activityTilesList = new List<Tile>()
        {
            Tile.Create(0, 0), Tile.Create(1, 0), Tile.Create(2, 0),
            Tile.Create(0, 1),                    Tile.Create(2, 1),
            Tile.Create(0, 2), Tile.Create(1, 2), Tile.Create(2, 2),
        };

        var activityTiles = ActivityTilesAggregate.Create(
            1,
            1,
            DateTime.UtcNow,
            prevTiles,
            activityTilesList);

        var activityTilesList2 = new List<Tile>()
        {
            Tile.Create(0, 0), Tile.Create(1, 0),
            Tile.Create(0, 1),
            Tile.Create(0, 2), Tile.Create(1, 2),
        };

        activityTiles.Update(prevTiles, activityTilesList2);

        Assert.Equal(1, activityTiles.NewSquare);
        Assert.Equal(5, activityTiles.Tiles.Count);
        Assert.Equal(4, activityTiles.NewTiles.Count);
        Assert.Empty(activityTiles.NewClusterTiles);
    }
}
