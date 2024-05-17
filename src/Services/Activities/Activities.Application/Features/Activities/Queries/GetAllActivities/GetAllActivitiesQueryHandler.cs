using Activities.Application.Dtos.Activities;
using Activities.Application.Interfaces;
using Common.Application.Interfaces;
using Common.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Features.Activities.Queries.GetAllActivities;
public sealed class GetAllActivitiesQueryHandler : IRequestHandler<GetAllActivitiesQuery, IEnumerable<ActivityResponse>>
{
    private readonly ILogger<GetAllActivitiesQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdProvider _userIdProvider;
    private readonly IMapper _mapper;

    public GetAllActivitiesQueryHandler(ILogger<GetAllActivitiesQueryHandler> logger, IUnitOfWork unitOfWork, IUserIdProvider userIdProvider, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userIdProvider = userIdProvider;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ActivityResponse>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
    {
        var stravaUserId = _userIdProvider.GetUserId();

        _logger.LogInformation("Fetching activities for athlete:{AthleteId}", stravaUserId);

        var activities = await _unitOfWork.Activities.GetAllAsync(
            filter: e => e.StravaUserId == stravaUserId,
            orderBy: e => e.Time.StartDate,
            sortOrder: SortOrder.Desc,
            cancellationToken: cancellationToken);

        var activityDtos = _mapper.Map<List<ActivityResponse>>(activities);

        return activityDtos;
    }
}
