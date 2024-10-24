using Common.Domain.Models;
using MediatR;

namespace Tiles.Application.Features.ActivityTiles.Commands.Add;
public record AddActivityTilesCommand(
    long StravaUserId,
    long StravaActivityId,
    DateTime CreatedAt,
    List<LatLng> LatLngs) : IRequest<Unit>;