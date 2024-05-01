using MediatR;
using Tiles.Application.Dtos.ActivityTiles;

namespace Tiles.Application.Features.ActivityTiles.Queries.GetAll;
public record GetAllActivityTilesQuery() : IRequest<IEnumerable<ActivityTilesResponse>>;
