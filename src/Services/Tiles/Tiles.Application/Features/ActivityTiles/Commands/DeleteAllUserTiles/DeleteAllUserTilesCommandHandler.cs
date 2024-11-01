using MediatR;
using Microsoft.Extensions.Logging;
using Tiles.Application.Interfaces;

namespace Tiles.Application.Features.ActivityTiles.Commands.DeleteAllUserTiles;
internal sealed class DeleteAllUserTilesCommandHandler : IRequestHandler<DeleteAllUserTilesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAllUserTilesCommandHandler> _logger;

    public DeleteAllUserTilesCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAllUserTilesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteAllUserTilesCommand request, CancellationToken cancellationToken)
    {
        var tiles = await _unitOfWork.Tiles.GetAllAsync(
            t => t.StravaUserId == request.StravaUserId,
            cancellationToken: cancellationToken);

        var activityIds = tiles.Select(x => x.StravaActivityId).ToList();
        var coordinates = await _unitOfWork.Coordinates.GetAllAsync(
            c => activityIds.Contains(c.StravaActivityId),
            cancellationToken: cancellationToken);

        _unitOfWork.Tiles.DeleteRange(tiles);
        _unitOfWork.Coordinates.DeleteRange(coordinates);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User:{UserId} tiles and related coordinates deleted successfully.", request.StravaUserId);

        return Unit.Value;
    }
}
