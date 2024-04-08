using Activities.Application.Dtos.Activities;
using Activities.Application.Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Activities.Application.Features.Activities.Queries.GetAllActivities;
public sealed class GetAllActivitiesQueryHandler : IRequestHandler<GetAllActivitiesQuery, IEnumerable<ActivityResponse>>
{
    private readonly ILogger<GetAllActivitiesQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GetAllActivitiesQueryHandler(ILogger<GetAllActivitiesQueryHandler> logger, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ActivityResponse>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
    {
        var stringId = _httpContextAccessor.HttpContext!.User.Claims
            .First(e => e.Type == ClaimTypes.NameIdentifier).Value;

        var stravaUserId = long.Parse(stringId);
        _logger.LogInformation("Fetching activities for athlete:{AthleteId}", stravaUserId);

        var activities = await _unitOfWork.Activities.FindAllAsync(e => e.StravaUserId == stravaUserId, cancellationToken);
        var activityDtos = activities.Select(e => _mapper.Map<ActivityResponse>(e));

        return activityDtos;
    }
}
