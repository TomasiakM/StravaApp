using Common.Application.Interfaces;
using Common.Domain.Enums;
using MapsterMapper;
using MediatR;
using Tiles.Application.Interfaces;

namespace Tiles.Application.Features.ActivityTiles.Queries.GetAll;
internal sealed class GetAllActivityTilesQueryHandler : IRequestHandler<GetAllActivityTilesQuery, IEnumerable<GetAllActivityTilesQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserIdProvider _userIdProvider;

    public GetAllActivityTilesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserIdProvider userIdProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userIdProvider = userIdProvider;
    }

    public async Task<IEnumerable<GetAllActivityTilesQueryResponse>> Handle(GetAllActivityTilesQuery request, CancellationToken cancellationToken)
    {
        var stravaUserId = _userIdProvider.GetUserId();

        var activityTiles = await _unitOfWork.Tiles.GetAllAsync(
                filter: e => e.StravaUserId == stravaUserId,
                orderBy: e => e.CreatedAt,
                asSplitQuery: true,
                sortOrder: SortOrder.Desc,
                cancellationToken: cancellationToken);

        var dtos = _mapper.Map<IEnumerable<GetAllActivityTilesQueryResponse>>(activityTiles);

        return dtos;
    }
}
