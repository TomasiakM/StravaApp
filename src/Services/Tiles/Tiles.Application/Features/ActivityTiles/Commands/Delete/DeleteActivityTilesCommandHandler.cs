using Common.Domain.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Application.Features.ActivityTiles.Commands.Delete;
internal sealed class DeleteActivityTilesCommandHandler : IRequestHandler<DeleteActivityTilesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteActivityTilesCommandHandler> _logger;

    public DeleteActivityTilesCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteActivityTilesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteActivityTilesCommand request, CancellationToken cancellationToken)
    {
        var activityTilesList = await _unitOfWork.Tiles.GetAllAsync(
            filter: e => e.StravaUserId == request.StravaActivityId,
            orderBy: e => e.CreatedAt,
            asSplitQuery: true,
            cancellationToken: cancellationToken);

        bool isDeleted = false;
        var prevTiles = new HashSet<Tile>();
        foreach (var activityTiles in activityTilesList)
        {
            if (activityTiles.StravaActivityId == request.StravaActivityId)
            {
                _unitOfWork.Tiles.Delete(activityTiles);
                isDeleted = true;
                continue;
            }

            prevTiles.AddRange(activityTiles.Tiles);
            if (isDeleted)
            {
                activityTiles.Update(prevTiles, activityTiles.Tiles);
            }
        }

        var coordinates = await _unitOfWork.Coordinates.GetAsync(e => e.StravaActivityId == request.StravaActivityId);
        if (coordinates is not null)
        {
            _unitOfWork.Coordinates.Delete(coordinates);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Removed tiles for activity:{ActivityId}", request.StravaActivityId);

        return Unit.Value;
    }
}
