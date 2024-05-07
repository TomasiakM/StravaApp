using Common.Domain.DDD;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Extensions.TileExtensions;

namespace Tiles.Domain.Aggregates.ActivityTiles;
public sealed class ActivityTilesAggregate : AggregateRoot<ActivityTilesId>
{
    private List<Tile> _tiles = new();
    private List<NewTile> _newTiles = new();
    private List<NewClusterTile> _newClusterTiles = new();
    private List<NewSquareTile> _newSquareTiles = new();

    public long StravaActivityId { get; init; }
    public long StravaUserId { get; init; }
    public DateTime CreatedAt { get; init; }

    public int NewSquare { get; private set; }

    public IReadOnlyList<Tile> Tiles => _tiles.AsReadOnly();
    public IReadOnlyList<NewTile> NewTiles => _newTiles.AsReadOnly();
    public IReadOnlyList<NewClusterTile> NewClusterTiles => _newClusterTiles.AsReadOnly();
    public IReadOnlyList<NewSquareTile> NewSquareTiles => _newSquareTiles.AsReadOnly();


    private ActivityTilesAggregate(long stravaActivityId, long stravaUserId, DateTime createdAt, IEnumerable<Tile> previousTiles, IEnumerable<Tile> activityTiles)
        : base(ActivityTilesId.Create())
    {
        StravaActivityId = stravaActivityId;
        StravaUserId = stravaUserId;
        CreatedAt = createdAt;

        NewSquare = previousTiles.Concat(activityTiles).MaxSquare() - previousTiles.MaxSquare();

        _tiles = activityTiles
            .ToList();

        _newTiles = previousTiles
            .FindNewTiles(activityTiles)
            .ToNewTiles()
            .ToList();

        _newClusterTiles = previousTiles
            .FindNewClusterTiles(activityTiles)
            .ToNewClusterTiles()
            .ToList();

        _newSquareTiles = previousTiles
            .FindNewSquareTiles(activityTiles)
            .ToNewSquareTiles()
            .ToList();
    }

    public static ActivityTilesAggregate Create(long stravaActivityId, long stravaUserId, DateTime createdAt, IEnumerable<Tile> previousTiles, IEnumerable<Tile> activityTiles)
        => new(stravaActivityId, stravaUserId, createdAt, previousTiles, activityTiles);

    public void Update(IEnumerable<Tile> previousTiles, IEnumerable<Tile> activityTiles)
    {
        NewSquare = previousTiles.Concat(activityTiles).MaxSquare() - previousTiles.MaxSquare();

        _tiles = activityTiles
            .ToList();

        _newTiles = previousTiles
            .FindNewTiles(activityTiles)
            .ToNewTiles()
            .ToList();

        _newClusterTiles = previousTiles
            .FindNewClusterTiles(activityTiles)
            .ToNewClusterTiles()
            .ToList();

        _newSquareTiles = previousTiles
            .FindNewSquareTiles(activityTiles)
            .ToNewSquareTiles()
            .ToList();
    }

    private ActivityTilesAggregate() : base(ActivityTilesId.Create()) { }
}
