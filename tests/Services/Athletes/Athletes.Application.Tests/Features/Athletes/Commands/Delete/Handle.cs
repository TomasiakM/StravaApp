using Athletes.Application.Features.Athletes.Commands.Delete;
using Athletes.Application.Interfaces;
using Athletes.Application.Tests.Common;
using Athletes.Domain.Aggregates.Athletes;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Athletes.Application.Tests.Features.Athletes.Commands.Delete;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldHandleDeleteAthleteCommandWhenAthleteExistsDeleteIt()
    {
        var stravaUserId = 4;
        var command = new DeleteAthleteCommand(stravaUserId);
        var athlete = Aggregates.CreateAthlete(stravaUserId);

        var handler = new DeleteAthleteCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteAthleteCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Athletes.GetAsync(
                It.IsAny<Expression<Func<AthleteAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(athlete);

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Athletes.Delete(It.IsAny<AthleteAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ShouldHandleDeleteAthleteCommandWhenAthleteIsNullDoNothing()
    {
        var command = new DeleteAthleteCommand(1);

        var handler = new DeleteAthleteCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteAthleteCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Athletes.GetAsync(
                It.IsAny<Expression<Func<AthleteAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync((AthleteAggregate?)null);

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Athletes.Delete(It.IsAny<AthleteAggregate>()), Times.Never);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Never);
    }
}
