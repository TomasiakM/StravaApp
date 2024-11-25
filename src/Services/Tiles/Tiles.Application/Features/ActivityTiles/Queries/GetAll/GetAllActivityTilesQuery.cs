using MediatR;

namespace Tiles.Application.Features.ActivityTiles.Queries.GetAll;
public record GetAllActivityTilesQuery() : IRequest<IEnumerable<GetAllActivityTilesQueryResponse>>;
