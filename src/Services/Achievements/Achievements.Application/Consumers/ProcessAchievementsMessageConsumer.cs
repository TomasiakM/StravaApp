using Achievements.Application.Interfaces;
using Achievements.Application.Interfaces.Services;
using Achievements.Domain.Interfaces;
using Common.Domain.Interfaces;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Achievements.Application.Consumers;
public sealed class ProcessAchievementsMessageConsumer : IConsumer<ProcessAchievementsMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAchievementFactory _achievementFactory;
    private readonly IDateProvider _dateProvider;
    private readonly IUserActivitiesService _userActivitiesService;
    private readonly ILogger<ProcessAchievementsMessageConsumer> _logger;
    private readonly IBus _bus;

    public ProcessAchievementsMessageConsumer(IUnitOfWork unitOfWork, IAchievementFactory achievementFactory, IDateProvider dateProvider, IUserActivitiesService userActivitiesService, ILogger<ProcessAchievementsMessageConsumer> logger, IBus bus)
    {
        _unitOfWork = unitOfWork;
        _achievementFactory = achievementFactory;
        _dateProvider = dateProvider;
        _userActivitiesService = userActivitiesService;
        _logger = logger;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ProcessAchievementsMessage> context)
    {
        var stravaUserId = context.Message.StravaUserId;

        _logger.LogInformation("Processing user:{UserId} achievements", stravaUserId);

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

        _logger.LogInformation("[BUS] Sending achievements process event");
        await _bus.Publish(new AchievementsProcessedEvent(
            context.Message.CorrelationId,
            context.Message.StravaActivityId,
            context.Message.StravaUserId));
    }
}
