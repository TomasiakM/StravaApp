using MediatR;

namespace Tiles.Application.Features.ActivityTiles.Commands.Delete;
public record DeleteActivityTilesCommand(
    long StravaActivityId) : IRequest<Unit>;
