using Common.Application.Interfaces;
using Common.Domain.Enums;
using MapsterMapper;
using Moq;
using System.Linq.Expressions;
using Tiles.Application.Features.ActivityTiles.Queries.GetAll;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Application.Tests.Features.ActivityTiles.Queries.GetAll;
public class Handler
{
    [Fact]
    public async Task SouldReturnActivityTilesResponseList()
    {
        var stravaUserId = 4;

        var mockRepository = new Mock<IActivityTilesRepository>();
        mockRepository
            .Setup(e => e.GetAllAsync(
                It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(),
                It.IsAny<Expression<Func<ActivityTilesAggregate, object>>>(),
                It.IsAny<SortOrder>(),
                It.IsAny<bool>(),
                default))
            .ReturnsAsync(new List<ActivityTilesAggregate> {
                ActivityTilesAggregate.Create(
                    1,
                    stravaUserId,
                    new DateTime(),
                    new List<Tile>(),
                    new List<Tile> { Tile.Create(1, 2), Tile.Create(1, 3) })
            });

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(e => e.Tiles).Returns(mockRepository.Object);

        var mockUserProvider = new Mock<IUserIdProvider>();
        mockUserProvider.Setup(e => e.GetUserId()).Returns(stravaUserId);

        var handler = new GetAllActivityTilesQueryHandler(
            mockUnitOfWork.Object,
            new Mapper(),
            mockUserProvider.Object);

        var result = await handler.Handle(new GetAllActivityTilesQuery(), default);

        Assert.Single(result);
        Assert.NotNull(result.First());
    }
}
