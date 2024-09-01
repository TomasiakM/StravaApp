using Achievements.Application.Interfaces;
using Achievements.Application.Interfaces.Services;
using Achievements.Domain.Interfaces;
using Common.Domain.Interfaces;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using MassTransit;

namespace Achievements.Application.Consumers;
public sealed class TilesProcessedEventConsumer : IConsumer<TilesProcessedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAchievementFactory _achievementFactory;
    private readonly IDateProvider _dateProvider;
    private readonly IUserActivitiesService _userActivitiesService;

    public TilesProcessedEventConsumer(IUnitOfWork unitOfWork, IAchievementFactory achievementFactory, IDateProvider dateProvider, IUserActivitiesService userActivitiesService)
    {
        _unitOfWork = unitOfWork;
        _achievementFactory = achievementFactory;
        _dateProvider = dateProvider;
        _userActivitiesService = userActivitiesService;
    }

    public async Task Consume(ConsumeContext<TilesProcessedEvent> context)
    {
        var stravaUserId = context.Message.StravaUserId;

        var userActivities = await _userActivitiesService.GetAllAsync(stravaUserId);
        var userAchievements = await _unitOfWork.Achievements
            .GetAllAsync(e => e.StravaUserId == stravaUserId);

        foreach (var achievement in userAchievements)
        {
            achievement.UpdateLevel(userActivities, _dateProvider);
        }

        var notSavedAchievements = _achievementFactory.CreateAll(stravaUserId, userAchievements);
        foreach (var achievement in notSavedAchievements)
        {
            achievement.UpdateLevel(userActivities, _dateProvider);
            _unitOfWork.Achievements.Add(achievement);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
