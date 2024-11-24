using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Tiles.Application.Features.ActivityTiles.Commands.DeleteAllUserTiles;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Tests.Features.ActivityTiles.Commands.DeleteAllUserTiles;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldDeleteAllUserTiles()
    {
        var command = new DeleteAllUserTilesCommand(1);

        var handler = new DeleteAllUserTilesCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteAllUserTilesCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Tiles.GetAllAsync(
                e => e.StravaUserId == command.StravaUserId,
                default, default, default, default))
            .ReturnsAsync(new List<ActivityTilesAggregate>());

        _unitOfWorkMock
            .Setup(e => e.Coordinates.GetAllAsync(
                It.IsAny<Expression<Func<CoordinatesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(new List<CoordinatesAggregate>());

        await handler.Handle(command, default);

        _unitOfWorkMock.VerifyAll();

        _unitOfWorkMock.Verify(e => e.Tiles.DeleteRange(It.IsAny<IEnumerable<ActivityTilesAggregate>>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Coordinates.DeleteRange(It.IsAny<IEnumerable<CoordinatesAggregate>>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }
}
