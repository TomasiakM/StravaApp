using Achievements.Application.Dtos.Achievements;
using Achievements.Application.Interfaces;
using Achievements.Domain.Interfaces;
using Common.Application.Interfaces;
using MapsterMapper;
using MediatR;

namespace Achievements.Application.Features.Achievements.Queries.GetAchievements;
internal sealed class GetAchievementsQueryHandler
    : IRequestHandler<GetAchievementsQuery, IEnumerable<AchievementsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdProvider _userIdProvider;
    private readonly IMapper _mapper;
    private readonly IAchievementFactory _achievementFactory;

    public GetAchievementsQueryHandler(IUnitOfWork unitOfWork, IUserIdProvider userIdProvider, IMapper mapper, IAchievementFactory achievementFactory)
    {
        _unitOfWork = unitOfWork;
        _userIdProvider = userIdProvider;
        _mapper = mapper;
        _achievementFactory = achievementFactory;
    }

    public async Task<IEnumerable<AchievementsResponse>> Handle(GetAchievementsQuery request, CancellationToken cancellationToken)
    {
        var stravaUserId = _userIdProvider.GetUserId();
        var achievements = await _unitOfWork.Achievements
            .GetAllAsync(e => e.StravaUserId == stravaUserId, cancellationToken: cancellationToken);

        var restAchievements = _achievementFactory.CreateAll(stravaUserId, achievements);

        var achievementDtos = _mapper.Map<IEnumerable<AchievementsResponse>>(achievements.Concat(restAchievements));

        return achievementDtos;
    }
}
