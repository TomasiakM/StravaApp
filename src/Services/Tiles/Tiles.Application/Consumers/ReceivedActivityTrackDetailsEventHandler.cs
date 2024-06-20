using Common.Domain.Extensions;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tiles.Application.Extensions;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Consumers;
public sealed class ReceivedActivityTrackDetailsEventHandler : IConsumer<ReceivedActivityTrackDetailsEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReceivedActivityTrackDetailsEventHandler> _logger;

    public ReceivedActivityTrackDetailsEventHandler(IUnitOfWork unitOfWork, ILogger<ReceivedActivityTrackDetailsEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReceivedActivityTrackDetailsEvent> context)
    {
        var coordinates = await _unitOfWork.Coordinates
            .GetAsync(e => e.StravaActivityId == context.Message.StravaActivityId);

        if (!IsRecalculationRequired(coordinates, context.Message.LatLngs))
        {
            _logger.LogInformation("Activity:{ActivityId} latlngs are the same, no need to recalculate it.", context.Message.StravaActivityId);
            return;
        }

        _logger.LogInformation("Calculating tiles for activity:{ActivityId}.", context.Message.StravaActivityId);
        var coordinatesCreated = coordinates is null;
        if (coordinates is null)
        {
            coordinates = CoordinatesAggregate.Create(context.Message.StravaActivityId, context.Message.LatLngs);
            _unitOfWork.Coordinates.Add(coordinates);
        }

        if (!coordinatesCreated)
        {
            coordinates.Update(context.Message.LatLngs);
        }

        var activityTilesList = await _unitOfWork.Tiles.GetAllAsync(
            filter: e => e.StravaUserId == context.Message.StravaUserId,
            orderBy: e => e.CreatedAt,
            asSplitQuery: true);

        if (IsListContainingActivity(context.Message, activityTilesList))
        {
            await HandleExistingActivityTiles(context.Message, activityTilesList);

            return;
        }

        await HandleNewActivityTiles(context.Message, activityTilesList);
    }

    private static bool IsRecalculationRequired(CoordinatesAggregate? coordinates, List<LatLng> latlngs)
    {
        return coordinates is not null &&
            JsonSerializer.Serialize(coordinates.Coordinates) != JsonSerializer.Serialize(latlngs);
    }

    private static bool IsListContainingActivity(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        return activityTilesList.Any(e => e.StravaActivityId == message.StravaActivityId);
    }

    private async Task HandleExistingActivityTiles(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        var prevTiles = new HashSet<Tile>();
        var activityUpdated = false;
        foreach (var actTiles in activityTilesList)
        {
            if (actTiles.StravaActivityId == message.StravaActivityId)
            {
                var tiles = message.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);
                actTiles.Update(prevTiles, tiles);

                prevTiles.AddRange(actTiles.Tiles);

                activityUpdated = true;

                continue;
            }

            if (!activityUpdated)
            {
                prevTiles.AddRange(actTiles.Tiles);

                continue;
            }

            actTiles.Update(prevTiles, actTiles.Tiles);
            prevTiles.AddRange(actTiles.Tiles);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task HandleNewActivityTiles(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        if (IsActivityLatest(message, activityTilesList))
        {
            var tiles = message.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);
            var newActivityTiles = ActivityTilesAggregate.Create(
                message.StravaActivityId,
                message.StravaUserId,
                message.CreatedAt,
                activityTilesList.SelectMany(e => e.Tiles).ToHashSet(),
                tiles);

            _unitOfWork.Tiles.Add(newActivityTiles);
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        var activityCreated = false;
        var prevTiles = new HashSet<Tile>();
        foreach (var actTiles in activityTilesList)
        {
            if (!activityCreated && actTiles.CreatedAt > message.CreatedAt)
            {
                var tiles = message.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);
                var newActivityTiles = ActivityTilesAggregate.Create(
                    message.StravaActivityId,
                    message.StravaUserId,
                    message.CreatedAt,
                    prevTiles,
                    tiles);

                _unitOfWork.Tiles.Add(newActivityTiles);
                activityCreated = true;

                prevTiles.AddRange(tiles);
            }

            if (!activityCreated)
            {
                prevTiles.AddRange(actTiles.Tiles);

                continue;
            }

            actTiles.Update(prevTiles, actTiles.Tiles);
            prevTiles.AddRange(actTiles.Tiles);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private bool IsActivityLatest(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        return activityTilesList.All(e => e.CreatedAt < message.CreatedAt);
    }
}
