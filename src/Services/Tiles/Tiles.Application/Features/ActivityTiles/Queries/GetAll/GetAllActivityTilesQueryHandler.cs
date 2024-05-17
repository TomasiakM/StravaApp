using Common.Application.Interfaces;
using MapsterMapper;
using MediatR;
using Tiles.Application.Dtos.ActivityTiles;
using Tiles.Application.Interfaces;

namespace Tiles.Application.Features.ActivityTiles.Queries.GetAll;
internal sealed class GetAllActivityTilesQueryHandler : IRequestHandler<GetAllActivityTilesQuery, IEnumerable<ActivityTilesResponse>>
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

    public async Task<IEnumerable<ActivityTilesResponse>> Handle(GetAllActivityTilesQuery request, CancellationToken cancellationToken)
    {
        var stravaUserId = _userIdProvider.GetUserId();

        var activityTiles = await _unitOfWork.Tiles
            .FindAllAsSplitQueryAsync(e => e.StravaUserId == stravaUserId);

        var dtos = _mapper.Map<IEnumerable<ActivityTilesResponse>>(activityTiles);

        return dtos;
    }
}
