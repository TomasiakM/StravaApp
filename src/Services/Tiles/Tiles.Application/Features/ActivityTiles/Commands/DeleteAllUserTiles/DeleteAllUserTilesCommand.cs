using MediatR;

namespace Tiles.Application.Features.ActivityTiles.Commands.DeleteAllUserTiles;
public record DeleteAllUserTilesCommand(
    long StravaUserId) : IRequest<Unit>;
