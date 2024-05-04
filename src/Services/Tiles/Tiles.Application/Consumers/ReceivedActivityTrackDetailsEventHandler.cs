using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Tiles.Application.Extensions;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;

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

        var tiles = context.Message.LatLngs.GetTiles();

        var activityTiles = await _unitOfWork.Tiles.FindAsync(e => e.StravaActivityId == context.Message.StravaActivityId);

        if (activityTiles is null)
        {
            activityTiles = ActivityTilesAggregate.Create(
                context.Message.StravaActivityId,
                context.Message.StravaUserId,
                context.Message.CreatedAt,
                tiles.ToList());

            _unitOfWork.Tiles.Add(activityTiles);
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        activityTiles.Update(tiles.ToList());
        await _unitOfWork.SaveChangesAsync();
    }
}
