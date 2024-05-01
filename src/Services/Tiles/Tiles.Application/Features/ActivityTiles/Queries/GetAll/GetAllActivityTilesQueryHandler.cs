using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tiles.Application.Dtos.ActivityTiles;
using Tiles.Application.Interfaces;

namespace Tiles.Application.Features.ActivityTiles.Queries.GetAll;
internal sealed class GetAllActivityTilesQueryHandler : IRequestHandler<GetAllActivityTilesQuery, IEnumerable<ActivityTilesResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllActivityTilesQueryHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ActivityTilesResponse>> Handle(GetAllActivityTilesQuery request, CancellationToken cancellationToken)
    {
        var stringId = _httpContextAccessor.HttpContext!.User.Claims
            .First(e => e.Type == ClaimTypes.NameIdentifier).Value;
        var stravaUserId = long.Parse(stringId);

        var activityTiles = await _unitOfWork.Tiles.FindAllAsync(e => e.StravaUserId == stravaUserId);
        var dtos = _mapper.Map<IEnumerable<ActivityTilesResponse>>(activityTiles);

        return dtos;
    }
}
