using Activities.Application.Features.Activities.Commands.DeleteAllUserActivities;
using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Streams;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Activities.Application.Tests.Features.Activities.Commands.DeleteAllUserActivities;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldDeleteAllActivitiesWithRelatedStreams()
    {
        var stravaUserId = 4;
        var command = new DeleteAllUserActivitiesCommand(stravaUserId);

        var handler = new DeleteAllUserActivitiesCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteAllUserActivitiesCommandHandler>>());

        _unitOfWorkMock.Setup(e => e.Activities).Returns(Mock.Of<IActivityRepository>());
        _unitOfWorkMock.Setup(e => e.Streams).Returns(Mock.Of<IStreamRepository>());

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e =>
            e.Activities.GetAllAsync(
                e => e.StravaUserId == command.StravaUserId,
                default, default, default, default),
            Times.Once);

        _unitOfWorkMock.Verify(e =>
            e.Streams.GetAllAsync(
                It.IsAny<Expression<Func<StreamAggregate, bool>>>(),
                default, default, default, default),
            Times.Once);

        _unitOfWorkMock.Verify(e => e.Activities.DeleteRange(It.IsAny<IEnumerable<ActivityAggregate>>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Streams.DeleteRange(It.IsAny<IEnumerable<StreamAggregate>>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }
}
