using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tiles.Application.Interfaces;
using Tiles.Application.Utils.ReceivedActivityTrackDetailsEventUtils;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Consumers;
public sealed class ActivityProcessedEventHandler : IConsumer<ActivityProcessedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivityProcessedEventHandler> _logger;
    private readonly NewActivityTilesHandler _newActivityTilesHandler;
    private readonly ExistingActivityTilesHandler _existingActivityTilesHandler;

    public ActivityProcessedEventHandler(IUnitOfWork unitOfWork, ILogger<ActivityProcessedEventHandler> logger, NewActivityTilesHandler newActivityTilesHandler, ExistingActivityTilesHandler existingActivityTilesHandler)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newActivityTilesHandler = newActivityTilesHandler;
        _existingActivityTilesHandler = existingActivityTilesHandler;
    }

    public async Task Consume(ConsumeContext<ActivityProcessedEvent> context)
    {
        var coordinates = await _unitOfWork.Coordinates
            .GetAsync(e => e.StravaActivityId == context.Message.StravaActivityId);

        if (!IsRecalculationRequired(coordinates, context.Message.LatLngs))
        {
            _logger.LogInformation("Activity:{ActivityId} latlngs are the same, no need to recalculate it.", context.Message.StravaActivityId);
            return;
        }

        CreateOrUpdateCoordinates(context.Message, coordinates);

        _logger.LogInformation("Calculating tiles for activity:{ActivityId}.", context.Message.StravaActivityId);

        var activityTilesList = await _unitOfWork.Tiles.GetAllAsync(
            filter: e => e.StravaUserId == context.Message.StravaUserId,
            orderBy: e => e.CreatedAt,
            asSplitQuery: true);

        if (IsActivityTilesExists(context.Message.StravaActivityId, activityTilesList))
        {
            _existingActivityTilesHandler.UpdateAggregates(context.Message, activityTilesList);
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        _newActivityTilesHandler.UpdateAggregates(context.Message, activityTilesList);
        await _unitOfWork.SaveChangesAsync();
    }

    private void CreateOrUpdateCoordinates(ActivityProcessedEvent message, CoordinatesAggregate? coordinates)
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
