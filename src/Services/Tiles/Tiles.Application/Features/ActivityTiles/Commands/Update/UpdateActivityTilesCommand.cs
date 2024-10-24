using Common.Domain.Models;
using MediatR;

namespace Tiles.Application.Features.ActivityTiles.Commands.Update;
public record UpdateActivityTilesCommand(
    long StravaUserId,
    long StravaActivityId,
    DateTime CreatedAt,
    List<LatLng> LatLngs) : IRequest<Unit>;
