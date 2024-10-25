using Common.Domain.Models;
using MediatR;

namespace Tiles.Application.Features.ActivityTiles.Commands.Create;
public record CreateActivityTilesCommand(
    long StravaUserId,
    long StravaActivityId,
    DateTime CreatedAt,
    List<LatLng> LatLngs) : IRequest<Unit>;