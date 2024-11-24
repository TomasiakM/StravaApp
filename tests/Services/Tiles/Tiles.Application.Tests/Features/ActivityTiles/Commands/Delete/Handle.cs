using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Tiles.Application.Features.ActivityTiles.Commands.Delete;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Tests.Features.ActivityTiles.Commands.Delete;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldAppropriateActivityTiles(List<ActivityTilesAggregate> tiles, CoordinatesAggregate? coordinates, int activityIdToDelete, int updateCount)
    {
        var command = new DeleteActivityTilesCommand(activityIdToDelete);

        var handler = new DeleteActivityTilesCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteActivityTilesCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Tiles.GetAllAsync(
                It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(),
                It.IsAny<Expression<Func<ActivityTilesAggregate, object>>>(),
                default, It.IsAny<bool>(), default))
            .ReturnsAsync(tiles);

        _unitOfWorkMock
            .Setup(e => e.Coordinates.GetAsync(
                It.IsAny<Expression<Func<CoordinatesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(coordinates);

        await handler.Handle(command, default);

        _unitOfWorkMock.VerifyAll();


        if (coordinates is not null)
        {
            _unitOfWorkMock.Verify(e => e.Coordinates.Delete(coordinates), Times.Once);
        }
        else
        {
            _unitOfWorkMock.Verify(e => e.Coordinates.Delete(It.IsAny<CoordinatesAggregate>()), Times.Never);
        }

        _unitOfWorkMock.Verify(e => e.Tiles.Update(It.IsAny<ActivityTilesAggregate>()), Times.Exactly(updateCount));
        _unitOfWorkMock.Verify(e => e.Tiles.Delete(It.IsAny<ActivityTilesAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    public static IEnumerable<object?[]> TestCases()
    {
        yield return new object?[] {
            new List<ActivityTilesAggregate>()
            {
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 1), new List<Tile>(), new List<Tile>()),
            },
            CoordinatesAggregate.Create(1, new()),
            1,
            0
        };

        yield return new object?[] {
            new List<ActivityTilesAggregate>()
            {
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 1), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(2, 1, new DateTime(2022, 1, 2), new List<Tile>(), new List<Tile>()),
            },
            CoordinatesAggregate.Create(1, new()),
            2,
            0
        };

        yield return new object?[] {
            new List<ActivityTilesAggregate>()
            {
                ActivityTilesAggregate.Create(1, 1, new DateTime(2022, 1, 1), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(2, 1, new DateTime(2022, 1, 2), new List<Tile>(), new List<Tile>()),
                ActivityTilesAggregate.Create(3, 1, new DateTime(2022, 1, 3), new List<Tile>(), new List<Tile>()),
            },
            null,
            2,
            1
        };
    }
}