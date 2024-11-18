using Athletes.Application.Features.Athletes.Commands.Create;
using Athletes.Application.Interfaces;
using Athletes.Domain.Aggregates.Athletes;
using Microsoft.Extensions.Logging;
using Moq;

namespace Athletes.Application.Tests.Features.Athletes.Commands.Create;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldhandleCreateAthleteCommand()
    {
        var command = new CreateAthleteCommand(1, "Username", "First", "Last", new DateTime(2022, 1, 1), "ProfileMedium", "Profile");

        var handler = new CreateAthleteCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<CreateAthleteCommandHandler>>());

        _unitOfWorkMock.Setup(e => e.Athletes).Returns(Mock.Of<IAthleteRepository>());

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Athletes.Add(It.IsAny<AthleteAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }
}
