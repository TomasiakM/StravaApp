using Common.Domain.Enums;
using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tiles.Application.Interfaces;
using Tiles.Application.Utils.ReceivedActivityTrackDetailsEventUtils;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Consumers;
public sealed class ProcessTilesMessageConsumer : IConsumer<ProcessTilesMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProcessTilesMessageConsumer> _logger;
    private readonly NewActivityTilesHandler _newActivityTilesHandler;
    private readonly ExistingActivityTilesHandler _existingActivityTilesHandler;
    private readonly IBus _bus;

    public ProcessTilesMessageConsumer(IUnitOfWork unitOfWork, ILogger<ProcessTilesMessageConsumer> logger, NewActivityTilesHandler newActivityTilesHandler, ExistingActivityTilesHandler existingActivityTilesHandler, IBus bus)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newActivityTilesHandler = newActivityTilesHandler;
        _existingActivityTilesHandler = existingActivityTilesHandler;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ProcessTilesMessage> context)
    {
        var coordinates = await _unitOfWork.Coordinates
            .GetAsync(e => e.StravaActivityId == context.Message.StravaActivityId);

        if (!CanProcessActivityTiles(context.Message.SportType))
        {
            if (await _unitOfWork.Tiles.AnyAsync(e => e.StravaActivityId == context.Message.StravaActivityId))
            {
                await RemoveActivityTile(context.Message);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Removed tiles for activity:{ActivityId}", context.Message.StravaActivityId);
            }

            _logger.LogInformation("Activity tiles processing cannot be done for {SportType} sport type", context.Message.SportType);

            return;
        }

        if (IsRecalculationRequired(coordinates, context.Message.LatLngs))
        {
            CreateOrUpdateCoordinates(context.Message, coordinates);

            _logger.LogInformation("Calculating tiles for activity:{ActivityId}.", context.Message.StravaActivityId);

            var activityTilesList = await _unitOfWork.Tiles.GetAllAsync(
                filter: e => e.StravaUserId == context.Message.StravaUserId,
                orderBy: e => e.CreatedAt,
                asSplitQuery: true);

            if (IsActivityTilesExists(context.Message.StravaActivityId, activityTilesList))
            {
                _existingActivityTilesHandler.UpdateAggregates(context.Message, activityTilesList);
            }
            else
            {
                _newActivityTilesHandler.UpdateAggregates(context.Message, activityTilesList);
            }

            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Activity:{ActivityId} latlngs are the same, no need to recalculate it.", context.Message.StravaActivityId);
        }

        _logger.LogInformation("[BUS] Sending tiles processed event.");
        await _bus.Publish(new TilesProcessedEvent(
            context.Message.CorrelationId,
            context.Message.StravaActivityId,
            context.Message.StravaUserId));
    }

    private async Task RemoveActivityTile(ProcessTilesMessage message)
    {
        var activityTilesList = await _unitOfWork.Tiles.GetAllAsync(
                            filter: e => e.StravaUserId == message.StravaUserId,
                            orderBy: e => e.CreatedAt,
                            asSplitQuery: true);

        bool tilesDeleted = false;
        var prevTiles = new HashSet<Tile>();
        foreach (var activityTiles in activityTilesList)
        {
            if (activityTiles.StravaActivityId == message.StravaActivityId)
            {
                _unitOfWork.Tiles.Delete(activityTiles);
                tilesDeleted = true;
                continue;
            }

            if (tilesDeleted)
            {
                activityTiles.Update(prevTiles, activityTiles.Tiles);
                continue;
            }
        }
    }

    private static bool CanProcessActivityTiles(SportType sportType)
    {
        return sportType switch
        {
            SportType.EBikeRide or
            SportType.EMountainBikeRide or
            SportType.VirtualRide or
            SportType.VirtualRow or
            SportType.VirtualRun
                => false,
            _ => true,
        };
    }

    private void CreateOrUpdateCoordinates(ProcessTilesMessage message, CoordinatesAggregate? coordinates)
    {
        if (coordinates is null)
        {
            _unitOfWork.Coordinates.Add(
                CoordinatesAggregate.Create(message.StravaActivityId, message.LatLngs));

            return;
        }

        coordinates.Update(message.LatLngs);
    }

    private static bool IsRecalculationRequired(CoordinatesAggregate? coordinates, List<LatLng> latlngs)
    {
        return coordinates is null ||
            JsonSerializer.Serialize(coordinates.Coordinates) != JsonSerializer.Serialize(latlngs);
    }

    private static bool IsActivityTilesExists(long stravaActivityId, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        return activityTilesList.Any(e => e.StravaActivityId == stravaActivityId);
    }
}
