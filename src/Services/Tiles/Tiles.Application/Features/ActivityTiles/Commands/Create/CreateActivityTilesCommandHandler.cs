using Common.Domain.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Tiles.Application.Extensions;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Features.ActivityTiles.Commands.Create;
internal sealed class CreateActivityTilesCommandHandler : IRequestHandler<CreateActivityTilesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateActivityTilesCommandHandler> _logger;

    public CreateActivityTilesCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateActivityTilesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateActivityTilesCommand request, CancellationToken cancellationToken)
    {
        var coordinates = CoordinatesAggregate.Create(request.StravaActivityId, request.LatLngs);
        _unitOfWork.Coordinates.Add(coordinates);

        var activityTilesList = await _unitOfWork.Tiles.GetAllAsync(e => e.StravaUserId == request.StravaUserId);
        if (!activityTilesList.Any() || !activityTilesList.Any(e => e.CreatedAt > request.CreatedAt))
        {
            var activityTiles = ActivityTilesAggregate.Create(
                request.StravaActivityId,
                request.StravaUserId,
                request.CreatedAt,
                activityTilesList.SelectMany(e => e.Tiles).ToHashSet(),
                request.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM));

            _unitOfWork.Tiles.Add(activityTiles);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Latest activity tiles created.");

            return Unit.Value;
        }

        var prevTiles = new HashSet<Tile>();
        var isCreated = false;
        foreach (var activityTiles in activityTilesList.OrderBy(e => e.CreatedAt))
        {
            if (!isCreated && activityTiles.CreatedAt > request.CreatedAt)
            {
                var tiles = request.LatLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);
                var newActivityTiles = ActivityTilesAggregate.Create(
                    request.StravaActivityId,
                    request.StravaUserId,
                    request.CreatedAt,
                    prevTiles,
                    tiles);

                _unitOfWork.Tiles.Add(newActivityTiles);
                prevTiles.AddRange(newActivityTiles.Tiles);

                isCreated = true;
            }

            if (!isCreated)
            {
                prevTiles.AddRange(activityTiles.Tiles);
                continue;
            }

            activityTiles.Update(prevTiles, activityTiles.Tiles);
            _unitOfWork.Tiles.Update(activityTiles);

            prevTiles.AddRange(activityTiles.Tiles);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Activity tiles created.");

        return Unit.Value;
    }
}
