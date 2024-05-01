using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Tiles.Application.Extensions;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;

namespace Tiles.Application.Consumers;
public sealed class ReceivedActivityTrackDetailsEventHandler : IConsumer<ReceivedActivityTrackDetailsEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReceivedActivityTrackDetailsEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ReceivedActivityTrackDetailsEvent> context)
    {
        var tiles = context.Message.LatLngs.GetTiles();

        var activityTiles = ActivityTilesAggregate.Create(
            context.Message.StravaActivityId,
            context.Message.StravaUserId,
            context.Message.CreatedAt,
            tiles.ToList());

        _unitOfWork.Tiles.Add(activityTiles);
        await _unitOfWork.SaveChangesAsync();
    }
}
