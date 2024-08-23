using Achievements.Application.Features.Achievements.Queries.GetAchievements;
using Achievements.Application.Interfaces;
using Achievements.Application.MapperConfigurations;
using Achievements.Domain.Aggregates.Achievement;
using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Achievements.Domain.Aggregates.Achievement.Factories;
using Common.Domain.Enums;
using Common.Tests.Utils;
using Moq;
using System.Linq.Expressions;

namespace Achievements.Application.Tests.Features.Achievements.Queries.GetAchievements;
public class Handler
{
    [Fact]
    public async Task ShouldReturnAchievements()
    {
        var userId = 123;

        var mockRepository = new Mock<IAchievementRepository>();
        mockRepository
            .Setup(e => e.GetAllAsync(
                It.IsAny<Expression<Func<Achievement, bool>>>(),
                It.IsAny<Expression<Func<Achievement, object>>>(),
                It.IsAny<SortOrder>(),
                It.IsAny<bool>(),
                default))
            .ReturnsAsync(new List<Achievement> { new CumulativeDistanceAchievement(userId) });

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork
            .Setup(e => e.Achievements)
            .Returns(mockRepository.Object);

        var handler = new GetAchievementsQueryHandler(
            mockUnitOfWork.Object,
            UserIdProviderFactory.Create(userId),
            MapperFactory.Create(typeof(AchievementsConfiguration).Assembly),
            new AchievementFactory());

        var result = await handler.Handle(new GetAchievementsQuery(), default);

        var allAchievements = new AchievementFactory().CreateAll(userId);

        Assert.Equal(allAchievements.Count, result.Count());
        Assert.Equal(allAchievements.Count, result.DistinctBy(e => e.AchievementType).Count());
    }
}
