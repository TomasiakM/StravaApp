using Common.Domain.Enums;
using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tiles.Application.Features.ActivityTiles.Commands.Add;
using Tiles.Application.Features.ActivityTiles.Commands.Delete;
using Tiles.Application.Features.ActivityTiles.Commands.Update;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Consumers;
public sealed class ProcessTilesMessageConsumer : IConsumer<ProcessTilesMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProcessTilesMessageConsumer> _logger;
    private readonly IBus _bus;
    private readonly ISender _sender;

    public ProcessTilesMessageConsumer(IUnitOfWork unitOfWork, ILogger<ProcessTilesMessageConsumer> logger, IBus bus, ISender sender)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _bus = bus;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<ProcessTilesMessage> context)
    {
        if (!IsSportTypeAllowedToProcessTiles(context.Message.SportType))
        {
            if (await _unitOfWork.Tiles.AnyAsync(e => e.StravaActivityId == context.Message.StravaActivityId))
            {
                await _sender.Send(new DeleteActivityTilesCommand(context.Message.StravaActivityId));
            }

            _logger.LogInformation("Activity tiles processing cannot be done for {SportType} sport type", context.Message.SportType);

            _logger.LogInformation("[BUS] Sending {Event}.", nameof(TilesProcessedEvent));
            await _bus.Publish(new TilesProcessedEvent(
                context.Message.CorrelationId,
                context.Message.StravaActivityId,
                context.Message.StravaUserId));

            return;
        }

        var coordinates = await _unitOfWork.Coordinates
            .GetAsync(e => e.StravaActivityId == context.Message.StravaActivityId);
        if (IsRecalculationRequired(coordinates, context.Message.LatLngs))
        {
            if (await _unitOfWork.Tiles.AnyAsync(e => e.StravaActivityId == context.Message.StravaActivityId))
            {
                await _sender.Send(new UpdateActivityTilesCommand(
                    context.Message.StravaUserId,
                    context.Message.StravaActivityId,
                    context.Message.CreatedAt,
                    context.Message.LatLngs));
            }
            else
            {
                await _sender.Send(new AddActivityTilesCommand(
                    context.Message.StravaUserId,
                    context.Message.StravaActivityId,
                    context.Message.CreatedAt,
                    context.Message.LatLngs));
            }
        }
        else
        {
            _logger.LogInformation("Activity:{ActivityId} latlngs are the same, no need to recalculate it.", context.Message.StravaActivityId);
        }

        _logger.LogInformation("[BUS] Sending {Event}.", nameof(TilesProcessedEvent));
        await _bus.Publish(new TilesProcessedEvent(
            context.Message.CorrelationId,
            context.Message.StravaActivityId,
            context.Message.StravaUserId));
    }

    private static bool IsSportTypeAllowedToProcessTiles(SportType sportType)
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

    private static bool IsRecalculationRequired(CoordinatesAggregate? coordinates, List<LatLng> latlngs)
    {
        return coordinates is null ||
            JsonSerializer.Serialize(coordinates.Coordinates) != JsonSerializer.Serialize(latlngs);
    }
}
