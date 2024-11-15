using Achievements.Application.Features.Achievements.Commands.Calculate;
using Achievements.Application.Interfaces;
using Achievements.Application.Interfaces.Services;
using Achievements.Domain.Aggregates.Achievement;
using Achievements.Domain.Aggregates.Achievement.Factories;
using Achievements.Domain.Interfaces;
using Common.Domain.Interfaces;
using Common.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Achievements.Application.Tests.Features.Achievements.Commands.Calculate;
public class Handler
{
    private readonly Mock<IAchievementFactory> _achievementFactoryMock = new();
    private readonly Mock<IUserActivitiesService> _userActivitiesServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IDateProvider> _dateProviderMock = new();

    private readonly IAchievementFactory _achievementFactory = new AchievementFactory();

    [Theory]
    [InlineData(2)]
    [InlineData(428)]
    public async Task ShouldHandleCalculateAchievementsCommand(long stravaUserId)
    {
        var command = new CalculateAchievementsCommand(stravaUserId);

        _achievementFactoryMock
            .Setup(e => e.CreateAll(stravaUserId, new List<Achievement>()))
            .Returns(new AchievementFactory().CreateAll(stravaUserId));

        _userActivitiesServiceMock
            .Setup(e => e.GetAllAsync(stravaUserId))
            .ReturnsAsync(new List<Activity>());

        _unitOfWorkMock
            .Setup(
                uow => uow.Achievements.GetAllAsync(
                    e => e.StravaUserId == command.StravaUserId,
                    default,
                    default,
                    default,
                    default))
            .ReturnsAsync(new List<Achievement>());

        var date = new DateTimeOffset(2022, 1, 1, 12, 0, 0, TimeSpan.Zero);
        _dateProviderMock
            .Setup(e => e.OffsetUtcNow)
            .Returns(date);

        var handler = new CalculateAchievementsCommandHandler(
            _unitOfWorkMock.Object,
            _achievementFactoryMock.Object,
            _dateProviderMock.Object,
            _userActivitiesServiceMock.Object,
            Mock.Of<ILogger<CalculateAchievementsCommandHandler>>());

        await handler.Handle(command, new());

        var allAchievements = new AchievementFactory().CreateAll(stravaUserId);

        _unitOfWorkMock.Verify(e => e.Achievements.Add(It.IsAny<Achievement>()), Times.Exactly(allAchievements.Count));
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);

        _unitOfWorkMock.VerifyAll();
        _achievementFactoryMock.VerifyAll();
        _userActivitiesServiceMock.VerifyAll();
    }

    [Theory]
    [InlineData(52)]
    [InlineData(448)]
    public async Task ShouldHandleCalculateAchievementsCommand2(long stravaUserId)
    {
        var command = new CalculateAchievementsCommand(stravaUserId);

        _userActivitiesServiceMock
            .Setup(e => e.GetAllAsync(stravaUserId))
            .ReturnsAsync(new List<Activity>());

        var achievementsFromDb = _achievementFactory.CreateAll(stravaUserId);
        _unitOfWorkMock
            .Setup(uow => uow.Achievements.GetAllAsync(
                e => e.StravaUserId == command.StravaUserId,
                default,
                default,
                default,
                default))
            .ReturnsAsync(achievementsFromDb);

        _achievementFactoryMock
            .Setup(e => e.CreateAll(stravaUserId, achievementsFromDb))
            .Returns(new List<Achievement>());

        var date = new DateTimeOffset(2022, 1, 1, 12, 0, 0, TimeSpan.Zero);
        _dateProviderMock
            .Setup(e => e.OffsetUtcNow)
            .Returns(date);

        var handler = new CalculateAchievementsCommandHandler(
            _unitOfWorkMock.Object,
            _achievementFactoryMock.Object,
            _dateProviderMock.Object,
            _userActivitiesServiceMock.Object,
            Mock.Of<ILogger<CalculateAchievementsCommandHandler>>());

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Achievements.Add(It.IsAny<Achievement>()), Times.Never);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);

        _unitOfWorkMock.VerifyAll();
        _achievementFactoryMock.VerifyAll();
        _userActivitiesServiceMock.VerifyAll();
    }
}
