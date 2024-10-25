using Achievements.Application.Interfaces;
using Achievements.Application.Interfaces.Services;
using Achievements.Domain.Interfaces;
using Common.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Achievements.Application.Features.Achievements.Commands.Calculate;
internal sealed class CalculateAchievementsCommandHandler : IRequestHandler<CalculateAchievementsCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAchievementFactory _achievementFactory;
    private readonly IDateProvider _dateProvider;
    private readonly IUserActivitiesService _userActivitiesService;
    private readonly ILogger _logger;

    public CalculateAchievementsCommandHandler(IUnitOfWork unitOfWork, IAchievementFactory achievementFactory, IDateProvider dateProvider, IUserActivitiesService userActivitiesService, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _achievementFactory = achievementFactory;
        _dateProvider = dateProvider;
        _userActivitiesService = userActivitiesService;
        _logger = logger;
    }

    public async Task<Unit> Handle(CalculateAchievementsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing user:{UserId} achievements", request.StravaUserId);

        var userActivities = await _userActivitiesService.GetAllAsync(request.StravaUserId);
        var userAchievements = await _unitOfWork.Achievements
            .GetAllAsync(e => e.StravaUserId == request.StravaUserId, cancellationToken: cancellationToken);

        foreach (var achievement in userAchievements)
        {
            achievement.UpdateLevel(userActivities, _dateProvider);
        }

        var newAchievements = _achievementFactory.CreateAll(request.StravaUserId, userAchievements);
        foreach (var newAchievement in newAchievements)
        {
            newAchievement.UpdateLevel(userActivities, _dateProvider);
            _unitOfWork.Achievements.Add(newAchievement);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Users achievements processed successfully.");

        return Unit.Value;
    }
}
