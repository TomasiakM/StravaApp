using Common.Domain.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Tiles.Application.Extensions;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Features.ActivityTiles.Commands.Update;
internal sealed class UpdateActivityTilesCommandHandler : IRequestHandler<UpdateActivityTilesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateActivityTilesCommandHandler> _logger;

    public UpdateActivityTilesCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateActivityTilesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateActivityTilesCommand request, CancellationToken cancellationToken)
    {
        var activityTilesList = await _unitOfWork.Tiles.GetAllAsync(e => e.StravaUserId == request.StravaUserId);
        var coordinates = await _unitOfWork.Coordinates.GetAsync(e => e.StravaActivityId == request.StravaActivityId);

        var isUpdated = false;
        var prevTiles = new HashSet<Tile>();
        foreach (var actTiles in activityTilesList)
        {
            if (actTiles.StravaActivityId == request.StravaActivityId)
            {
                var tiles = request.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);
                actTiles.Update(prevTiles, tiles);
                _unitOfWork.Tiles.Update(actTiles);

                prevTiles.AddRange(actTiles.Tiles);

                isUpdated = true;

                continue;
            }

            if (!isUpdated)
            {
                prevTiles.AddRange(actTiles.Tiles);
                continue;
            }

            actTiles.Update(prevTiles, actTiles.Tiles);
            _unitOfWork.Tiles.Update(actTiles);

            prevTiles.AddRange(actTiles.Tiles);
        }

        if (!isUpdated)
        {
            _logger.LogWarning("Activity:{ActivityId} not found", request.StravaActivityId);
        }

        if (coordinates is not null)
        {
            coordinates.Update(request.LatLngs);
            _unitOfWork.Coordinates.Update(coordinates);
        }
        else
        {
            _logger.LogWarning("Coordinates not found for activity:{ActivityId}, creating new record.", request.StravaActivityId);
            _unitOfWork.Coordinates.Add(CoordinatesAggregate.Create(request.StravaActivityId, request.LatLngs));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Activity tiles updated.");

        return Unit.Value;
    }
}
