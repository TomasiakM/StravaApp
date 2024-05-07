using Common.Domain.DDD;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Utils.Tiles;

namespace Tiles.Domain.Aggregates.ActivityTiles;
public sealed class ActivityTilesAggregate : AggregateRoot<ActivityTilesId>
{
    private List<Tile> _tiles = new();
    private List<Tile> _newTiles = new();
    private List<Tile> _newClusterTiles = new();
    private List<Tile> _newSquareTiles = new();

    public long StravaActivityId { get; init; }
    public long StravaUserId { get; init; }
    public DateTime CreatedAt { get; init; }

    public int NewSquare { get; private set; }

    public IReadOnlyList<Tile> Tiles => _tiles.AsReadOnly();
    public IReadOnlyList<Tile> NewTiles => _newTiles.AsReadOnly();
    public IReadOnlyList<Tile> NewClusterTiles => _newClusterTiles.AsReadOnly();
    public IReadOnlyList<Tile> NewSquareTiles => _newSquareTiles.AsReadOnly();


    private ActivityTilesAggregate(long stravaActivityId, long stravaUserId, DateTime createdAt, IEnumerable<Tile> previousTiles, IEnumerable<Tile> activityTiles)
        : base(ActivityTilesId.Create())
    {
        StravaActivityId = stravaActivityId;
        StravaUserId = stravaUserId;
        CreatedAt = createdAt;

        NewSquare = previousTiles.MaxSquare() - previousTiles.Concat(activityTiles).MaxSquare();

        _tiles = activityTiles
            .ToList();

        _newTiles = previousTiles
            .FindNewTiles(activityTiles)
            .ToList();

        _newClusterTiles = previousTiles
            .FindNewClusterTiles(activityTiles)
            .ToList();

        _newSquareTiles = previousTiles
            .FindNewSquareTiles(activityTiles)
            .ToList();
    }

    public static ActivityTilesAggregate Create(long stravaActivityId, long stravaUserId, DateTime createdAt, IEnumerable<Tile> previousTiles, IEnumerable<Tile> activityTiles)
        => new(stravaActivityId, stravaUserId, createdAt, previousTiles, activityTiles);

    public void Update(IEnumerable<Tile> previousTiles, IEnumerable<Tile> activityTiles)
    {
        NewSquare = previousTiles.MaxSquare() - previousTiles.Concat(activityTiles).MaxSquare();

        _tiles = activityTiles
            .ToList();

        _newTiles = previousTiles
            .FindNewTiles(activityTiles)
            .ToList();

        _newClusterTiles = previousTiles
            .FindNewClusterTiles(activityTiles)
            .ToList();

        _newSquareTiles = previousTiles
            .FindNewSquareTiles(activityTiles)
            .ToList();
    }

    private ActivityTilesAggregate() : base(ActivityTilesId.Create()) { }
}
