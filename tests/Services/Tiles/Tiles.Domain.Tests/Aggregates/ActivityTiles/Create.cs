using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Tests.Aggregates.ActivityTiles;
public class Create
{
    [Fact]
    public void ShouldCreateActivityTilesAggregate()
    {
        var stravaActivityId = 1;
        var stravaUserId = 1;
        var createdAt = DateTime.UtcNow;
        var activityTilesList = new List<Tile>()
        {
            Tile.Create(0, 0), Tile.Create(1, 0), Tile.Create(2, 0),
            Tile.Create(0, 1), Tile.Create(1, 1), Tile.Create(2, 1),
            Tile.Create(0, 2), Tile.Create(1, 2), Tile.Create(2, 2),
        };

        var activityTiles = ActivityTilesAggregate.Create(
            stravaActivityId,
            stravaUserId,
            createdAt,
            new List<Tile>(),
            activityTilesList);

        Assert.Equal(stravaActivityId, activityTiles.StravaActivityId);
        Assert.Equal(stravaUserId, activityTiles.StravaUserId);
        Assert.Equal(createdAt, activityTiles.CreatedAt);

        Assert.True(activityTilesList.SequenceEqual(activityTiles.Tiles));

        Assert.Equal(3, activityTiles.NewSquare);
        Assert.Equal(9, activityTiles.NewTiles.Count);
        Assert.Equal(1, activityTiles.NewClusterTiles.Count);
    }

    [Fact]
    public void ShouldCreateActivityTilesAggregateWithPreviousTiles()
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

        Assert.True(activityTilesList.SequenceEqual(activityTiles.Tiles));

        Assert.Equal(2, activityTiles.NewSquare);
        Assert.Equal(7, activityTiles.NewTiles.Count);
        Assert.Equal(1, activityTiles.NewClusterTiles.Count);
    }
}
