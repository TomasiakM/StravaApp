using Common.Domain.DDD;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Domain.Aggregates.ActivityTiles;
public sealed class ActivityTilesAggregate : AggregateRoot<ActivityTilesId>
{
    private List<Tile> _tiles = new();

    public long StravaActivityId { get; init; }
    public long StravaUserId { get; init; }
    public DateTime CreatedAt { get; init; }

    public IReadOnlyList<Tile> Tiles => _tiles.AsReadOnly();

    private ActivityTilesAggregate(long stravaActivityId, long stravaUserId, DateTime createdAt, ICollection<Tile> activityTiles)
        : base(ActivityTilesId.Create())
    {
        StravaActivityId = stravaActivityId;
        StravaUserId = stravaUserId;
        CreatedAt = createdAt;

        _tiles = activityTiles.ToList();
    }

    public static ActivityTilesAggregate Create(long stravaActivityId, long stravaUserId, DateTime createdAt, ICollection<Tile> activityTiles)
        => new(stravaActivityId, stravaUserId, createdAt, activityTiles);

    public void Update(ICollection<Tile> activityTiles)
    {
        _tiles = activityTiles.ToList();
    }
}
