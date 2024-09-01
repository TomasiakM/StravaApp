using Common.Domain.Extensions;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Tiles.Application.Extensions;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Application.Utils.ReceivedActivityTrackDetailsEventUtils;
public sealed class NewActivityTilesHandler
{
    private bool _isCreated = false;
    private readonly IUnitOfWork _unitOfWork;

    public NewActivityTilesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void UpdateAggregates(ActivityProcessedEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        if (IsActivityLatest(message, activityTilesList))
        {
            var activityTiles = ActivityTilesAggregate.Create(
                message.StravaActivityId,
                message.StravaUserId,
                message.CreatedAt,
                activityTilesList.SelectMany(e => e.Tiles).ToHashSet(),
                message.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM));

            _unitOfWork.Tiles.Add(activityTiles);
            return;
        }

        var previousTiles = new HashSet<Tile>();
        foreach (var activityTiles in activityTilesList.OrderBy(e => e.CreatedAt))
        {
            HandleActivityTilesUpdate(message, previousTiles, activityTiles);
        }
    }

    private void HandleActivityTilesUpdate(ActivityProcessedEvent message, HashSet<Tile> previousTiles, ActivityTilesAggregate activityTiles)
    {
        if (IsValidToCreate(message, activityTiles))
        {
            var newActivityTiles = CreateActivityTiles(message, previousTiles);

            _unitOfWork.Tiles.Add(newActivityTiles);
            previousTiles.AddRange(newActivityTiles.Tiles);

            _isCreated = true;
        }

        if (!_isCreated)
        {
            previousTiles.AddRange(activityTiles.Tiles);
            return;
        }

        activityTiles.Update(previousTiles, activityTiles.Tiles);
        previousTiles.AddRange(activityTiles.Tiles);
    }

    private static ActivityTilesAggregate CreateActivityTiles(ActivityProcessedEvent message, HashSet<Tile> previousTiles)
    {
        var tiles = message.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);
        var activityTiles = ActivityTilesAggregate.Create(
            message.StravaActivityId,
            message.StravaUserId,
            message.CreatedAt,
            previousTiles,
            tiles);

        return activityTiles;
    }

    private bool IsValidToCreate(ActivityProcessedEvent message, ActivityTilesAggregate actTiles)
    {
        return !_isCreated && actTiles.CreatedAt > message.CreatedAt;
    }

    private static bool IsActivityLatest(ActivityProcessedEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        return activityTilesList.All(e => e.CreatedAt < message.CreatedAt);
    }
}
