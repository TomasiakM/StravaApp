using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Tiles.Application.Features.ActivityTiles.Commands.Update;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Tests.Features.ActivityTiles.Commands.Update;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldUpdateTiles(List<ActivityTilesAggregate> tiles, CoordinatesAggregate? coordinates, int activityIdToUpdate, int expectedUpdates)
    {
        var command = new UpdateActivityTilesCommand(1, activityIdToUpdate, new DateTime(2022, 1, 1), new());

        var handler = new UpdateActivityTilesCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<UpdateActivityTilesCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Tiles.GetAllAsync(
                It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(tiles);

        _unitOfWorkMock
            .Setup(e => e.Coordinates.GetAsync(
                It.IsAny<Expression<Func<CoordinatesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(coordinates);


        await handler.Handle(command, default);

        if (coordinates is null)
        {
            _unitOfWorkMock.Verify(e => e.Coordinates.Add(It.IsAny<CoordinatesAggregate>()), Times.Once);
        }
        else
        {
            _unitOfWorkMock.Verify(e => e.Coordinates.Update(It.IsAny<CoordinatesAggregate>()), Times.Once);
        }

        _unitOfWorkMock.VerifyAll();
        _unitOfWorkMock.Verify(e => e.Tiles.Update(It.IsAny<ActivityTilesAggregate>()), Times.Exactly(expectedUpdates));
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    public static IEnumerable<object?[]> TestCases()
    {
        yield return new object?[] {
            new List<ActivityTilesAggregate>() {
                ActivityTilesAggregate.Create(1, 1, new(2022, 1, 1), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(2, 1, new(2022, 1, 2), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(3, 1, new(2022, 1, 3), new List<Tile>(), new List<Tile>())
            },
            CoordinatesAggregate.Create(1, new()),
            1,
            3
        };

        yield return new object?[] {
            new List<ActivityTilesAggregate>() {
                ActivityTilesAggregate.Create(1, 1, new(2022, 1, 1), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(2, 1, new(2022, 1, 2), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(3, 1, new(2022, 1, 3), new List<Tile>(), new List<Tile>())
            },
            CoordinatesAggregate.Create(2, new()),
            2,
            2
        };

        yield return new object?[] {
            new List<ActivityTilesAggregate>() {
                ActivityTilesAggregate.Create(1, 1, new(2022, 1, 1), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(2, 1, new(2022, 1, 2), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(3, 1, new(2022, 1, 3), new List<Tile>(), new List<Tile>())
            },
            null,
            3,
            1
        };
    }
}
