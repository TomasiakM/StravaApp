using Athletes.Application.Features.Athletes.Commands.Update;
using Athletes.Application.Interfaces;
using Athletes.Application.Tests.Common;
using Athletes.Domain.Aggregates.Athletes;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Athletes.Application.Tests.Features.Athletes.Commands.Update;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldHandleUpdateAthleteCommand()
    {
        var stravaUserId = 5;
        var command = new UpdateAthleteCommand(stravaUserId, "Username", "First", "Last", new DateTime(2022, 1, 1), "ProfileMedium", "Profile");
        var athlete = Aggregates.CreateAthlete(stravaUserId);

        var handler = new UpdateAthleteCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<UpdateAthleteCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Athletes.GetAsync(
                It.IsAny<Expression<Func<AthleteAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(athlete);

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Athletes.Update(It.IsAny<AthleteAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }
}
