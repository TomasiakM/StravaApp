using Achievements.Application.Features.Achievements.Commands.DeleteAllUserAchievements;
using Achievements.Application.Interfaces;
using Achievements.Domain.Aggregates.Achievement.Factories;
using Achievements.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Achievements.Application.Tests.Features.Achievements.Commands.DeleteAllUserAchievements;
public class Handler
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly IAchievementFactory _achievementFactory = new AchievementFactory();

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    public async Task ShouldHandleDeleteAllUserAchievementsCommand(long stravaUserId)
    {
        var command = new DeleteAllUserAchievementsCommand(stravaUserId);

        var allAchievements = _achievementFactory.CreateAll(stravaUserId);
        _unitOfWorkMock
            .Setup(e => e.Achievements.GetAllAsync(
                e => e.StravaUserId == command.StravaUserId,
                default,
                default,
                default,
                default))
            .ReturnsAsync(allAchievements);

        var handler = new DeleteAllUserAchievementsCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteAllUserAchievementsCommandHandler>>());

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Achievements.DeleteRange(allAchievements));
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default));

        _unitOfWorkMock.VerifyAll();
    }
}
