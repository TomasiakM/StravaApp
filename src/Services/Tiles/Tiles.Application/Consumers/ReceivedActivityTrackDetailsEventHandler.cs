using Common.Domain.Extensions;
using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Tiles.Application.Extensions;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

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
        _logger.LogInformation("Handling list of coordinates for activity:{ActivityId}.", context.Message.StravaActivityId);

        var activityTilesList = await _unitOfWork.Tiles.GetAllAsync();

        if (IsLatest(context.Message, activityTilesList))
        {
            await HandleLatestActivityTiles(context.Message, activityTilesList);
            return;
        }

        if (IsCreated(context.Message, activityTilesList))
        {
            await HandleExistingActivityTiles(context.Message, activityTilesList);
            return;
        }

        await HandleNewActivityTiles(context.Message, activityTilesList);
    }

    private static bool IsCreated(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        return activityTilesList.Any(e => e.StravaActivityId == message.StravaActivityId);
    }

    private async Task HandleLatestActivityTiles(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        var tiles = message.LatLngs.GetTiles();

        var previousTiles = activityTilesList.SelectMany(e => e.Tiles.ToList());
        var activityTiles = activityTilesList.FirstOrDefault(e => e.StravaActivityId == message.StravaActivityId);

        if (activityTiles is null)
        {
            activityTiles = ActivityTilesAggregate.Create(
                message.StravaActivityId,
                message.StravaUserId,
                message.CreatedAt,
                previousTiles,
                new List<Tile>());

            activityTiles.Update(previousTiles, tiles);

            _unitOfWork.Tiles.Add(activityTiles);
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        activityTiles.Update(previousTiles, tiles);
        await _unitOfWork.SaveChangesAsync();
    }

    private static bool IsLatest(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        return activityTilesList.All(e => e.CreatedAt < message.CreatedAt);
    }

    private async Task HandleExistingActivityTiles(ReceivedActivityTrackDetailsEvent message, IEnumerable<ActivityTilesAggregate> activityTilesList)
    {
        var prevTiles = new HashSet<Tile>();
        foreach (var actTiles in activityTilesList)
        {
            if (actTiles.StravaActivityId == message.StravaActivityId)
            {
                var tiles = message.LatLngs.GetTiles();
                actTiles.Update(prevTiles, tiles);

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
        var prevTiles = new HashSet<Tile>();
        var created = false;
        foreach (var actTiles in activityTilesList)
        {
            if (!created && actTiles.CreatedAt > message.CreatedAt)
            {
                created = true;

                var tiles = message.LatLngs.GetTiles();
                var newActivityTiles = ActivityTilesAggregate.Create(
                    message.StravaActivityId,
                    message.StravaUserId,
                    message.CreatedAt,
                    prevTiles,
                    tiles);

                _unitOfWork.Tiles.Add(newActivityTiles);
            }

            actTiles.Update(prevTiles, actTiles.Tiles);
            prevTiles.AddRange(actTiles.Tiles);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
