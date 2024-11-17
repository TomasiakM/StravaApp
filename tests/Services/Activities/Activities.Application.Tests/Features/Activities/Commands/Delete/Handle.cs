using Activities.Application.Features.Activities.Commands.Delete;
using Activities.Application.Interfaces;
using Activities.Application.Tests.Common;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Streams;
using Microsoft.Extensions.Logging;
using Moq;

namespace Activities.Application.Tests.Features.Activities.Commands.Delete;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldDeleteActivityWithRelatedStream()
    {
        var stravaActivityId = 4;
        var command = new DeleteActivityCommand(stravaActivityId);
        var activity = Aggregates.CreateActivity(stravaActivityId, 1);
        var stream = Aggregates.CreateStream(activity.Id);

        var handler = new DeleteActivityCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteActivityCommandHandler>>());

        _unitOfWorkMock.Setup(e => e.Activities).Returns(Mock.Of<IActivityRepository>());
        _unitOfWorkMock.Setup(e => e.Streams).Returns(Mock.Of<IStreamRepository>());

        _unitOfWorkMock
            .Setup(e => e.Activities.GetAsync(
                e => e.StravaId == command.StravaActivityId,
                default, default, default, default))
            .ReturnsAsync(activity);

        _unitOfWorkMock
            .Setup(e => e.Streams.GetAsync(
                e => e.ActivityId == activity.Id,
                default, default, default, default))
            .ReturnsAsync(stream);

        await handler.Handle(command, default);

        _unitOfWorkMock.VerifyAll();
        _unitOfWorkMock.Verify(e => e.Activities.Delete(It.IsAny<ActivityAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Streams.Delete(It.IsAny<StreamAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ShouldDeleteOnlyActivityWhenStreamIsNull()
    {
        var stravaActivityId = 4;
        var command = new DeleteActivityCommand(stravaActivityId);
        var activity = Aggregates.CreateActivity(stravaActivityId, 1);

        var handler = new DeleteActivityCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteActivityCommandHandler>>());

        _unitOfWorkMock.Setup(e => e.Activities).Returns(Mock.Of<IActivityRepository>());
        _unitOfWorkMock.Setup(e => e.Streams).Returns(Mock.Of<IStreamRepository>());

        _unitOfWorkMock
            .Setup(e => e.Activities.GetAsync(
                e => e.StravaId == command.StravaActivityId,
                default, default, default, default))
            .ReturnsAsync(activity);

        _unitOfWorkMock
            .Setup(e => e.Streams.GetAsync(
                e => e.ActivityId == activity.Id,
                default, default, default, default))
            .ReturnsAsync((StreamAggregate?)null);


        await handler.Handle(command, default);

        _unitOfWorkMock.VerifyAll();
        _unitOfWorkMock.Verify(e => e.Activities.Delete(It.IsAny<ActivityAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Streams.Delete(It.IsAny<StreamAggregate>()), Times.Never);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ShouldDeleteNotDeleteAnythingWhenActivityIsNull()
    {
        var stravaActivityId = 4;
        var command = new DeleteActivityCommand(stravaActivityId);

        var handler = new DeleteActivityCommandHandler(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<DeleteActivityCommandHandler>>());

        _unitOfWorkMock.Setup(e => e.Activities).Returns(Mock.Of<IActivityRepository>());

        _unitOfWorkMock
            .Setup(e => e.Activities.GetAsync(
                e => e.StravaId == command.StravaActivityId,
                default, default, default, default))
            .ReturnsAsync((ActivityAggregate?)null);

        await handler.Handle(command, default);

        _unitOfWorkMock.VerifyAll();
        _unitOfWorkMock.Verify(e => e.Activities.Delete(It.IsAny<ActivityAggregate>()), Times.Never);
        _unitOfWorkMock.Verify(e => e.Streams.Delete(It.IsAny<StreamAggregate>()), Times.Never);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }
}
