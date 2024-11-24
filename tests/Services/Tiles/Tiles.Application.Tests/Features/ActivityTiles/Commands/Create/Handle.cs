using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Tiles.Application.Features.ActivityTiles.Commands.Create;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Tests.Features.ActivityTiles.Commands.Create;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Theory]
    [MemberData(nameof(LatestActivityTestCases))]
    public async Task ShouldCreateTilesWhenNewOneIsLatest(List<ActivityTilesAggregate> tiles, DateTime newtileDate)
    {
        var command = new CreateActivityTilesCommand(1, 2, newtileDate, new());

        var handler = new CreateActivityTilesCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<CreateActivityTilesCommandHandler>>());

        _unitOfWorkMock.Setup(e => e.Coordinates).Returns(Mock.Of<ICoordinatesRepository>());
        _unitOfWorkMock
            .Setup(e => e.Tiles.GetAllAsync(
                It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(tiles);

        await handler.Handle(command, default);

        _unitOfWorkMock.VerifyAll();

        _unitOfWorkMock.Verify(e => e.Coordinates.Add(It.IsAny<CoordinatesAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Tiles.Add(It.IsAny<ActivityTilesAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ActivityNotLatestTestCases))]
    public async Task ShouldCreateTilesWhenNewOneIsNotLatest(List<ActivityTilesAggregate> tiles, DateTime newtileDate, int updateCount)
    {
        var command = new CreateActivityTilesCommand(1, 2, newtileDate, new());

        var handler = new CreateActivityTilesCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<CreateActivityTilesCommandHandler>>());

        _unitOfWorkMock.Setup(e => e.Coordinates).Returns(Mock.Of<ICoordinatesRepository>());
        _unitOfWorkMock
            .Setup(e => e.Tiles.GetAllAsync(
                It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(tiles);

        await handler.Handle(command, default);

        _unitOfWorkMock.VerifyAll();

        _unitOfWorkMock.Verify(e => e.Coordinates.Add(It.IsAny<CoordinatesAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Tiles.Add(It.IsAny<ActivityTilesAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Tiles.Update(It.IsAny<ActivityTilesAggregate>()), Times.Exactly(updateCount));
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    public static IEnumerable<object[]> LatestActivityTestCases()
    {
        yield return new object[] {
            new List<ActivityTilesAggregate>(),
            new DateTime(2022, 1, 1) };

        yield return new object[] {
            new List<ActivityTilesAggregate>() {
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 1), new List<Tile>(), new List<Tile>())
            },
            new DateTime(2022, 1, 2)
        };

        yield return new object[] {
            new List<ActivityTilesAggregate>() {
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 1), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 2), new List<Tile>(), new List<Tile>())
            },
            new DateTime(2022, 1, 3)
        };
    }

    public static IEnumerable<object[]> ActivityNotLatestTestCases()
    {
        yield return new object[] {
            new List<ActivityTilesAggregate>() {
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 1), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(1, 3, new DateTime(2022, 1, 3), new List<Tile>(), new List<Tile>())
            },
            new DateTime(2022, 1, 2),
            1
        };

        yield return new object[] {
            new List<ActivityTilesAggregate>() {
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 2), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 3), new List<Tile>(), new List<Tile>())
            },
            new DateTime(2022, 1, 1),
            2
        };
    }
}
