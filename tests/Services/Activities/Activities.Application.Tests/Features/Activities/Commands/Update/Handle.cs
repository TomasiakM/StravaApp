using Activities.Application.Factories;
using Activities.Application.Features.Activities.Commands.Update;
using Activities.Application.Interfaces;
using Activities.Application.Tests.Common;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace Activities.Application.Tests.Features.Activities.Commands.Update;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldUpdateActivityWithRelatedStream()
    {
        var stravaActivityId = 4;
        var command = CreateUpdateActivityCommand(stravaActivityId);
        var activity = Aggregates.CreateActivity(stravaActivityId, 1);
        var stream = Aggregates.CreateStream(activity.Id);

        var handler = new UpdateActivityCommandHandler(
            _unitOfWorkMock.Object,
            new ActivityAggregateFactory(),
            Mock.Of<ILogger<UpdateActivityCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Activities.GetAsync(
                e => e.StravaId == command.Id,
                default, default, default, default))
            .ReturnsAsync(activity);

        _unitOfWorkMock
            .Setup(e => e.Streams.GetAsync(
                e => e.ActivityId == activity.Id,
                default, default, default, default))
            .ReturnsAsync(stream);

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Activities.Update(It.IsAny<ActivityAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Activities.Update(It.IsAny<ActivityAggregate>()), Times.Once);

        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowsExceptionWhenActivityIsNull()
    {
        var command = CreateUpdateActivityCommand(1);

        var handler = new UpdateActivityCommandHandler(
            _unitOfWorkMock.Object,
            new ActivityAggregateFactory(),
            Mock.Of<ILogger<UpdateActivityCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Activities.GetAsync(
                e => e.StravaId == command.Id,
                default, default, default, default))
            .ReturnsAsync((ActivityAggregate?)null);

        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Never);

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task ShouldThrowsExceptionWhenStreamIsNull()
    {
        var stravaActivityId = 4;
        var command = CreateUpdateActivityCommand(stravaActivityId);
        var activity = Aggregates.CreateActivity(stravaActivityId, 1);

        var handler = new UpdateActivityCommandHandler(
            _unitOfWorkMock.Object,
            new ActivityAggregateFactory(),
            Mock.Of<ILogger<UpdateActivityCommandHandler>>());

        _unitOfWorkMock
            .Setup(e => e.Activities.GetAsync(
                e => e.StravaId == command.Id,
                default, default, default, default))
            .ReturnsAsync(activity);

        _unitOfWorkMock
            .Setup(e => e.Streams.GetAsync(
                e => e.ActivityId == activity.Id,
                default, default, default, default))
            .ReturnsAsync((StreamAggregate?)null);

        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Never);

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, default));
    }

    private static UpdateActivityCommand CreateUpdateActivityCommand(long stravaActivityId)
    {
        return new UpdateActivityCommand(stravaActivityId, "Test", 111, 222, 333, 444, SportType.Golf,
            new DateTime(1111111111), new DateTime(2222222222), new double[] { 1, 1 }, new double[] { 2, 2 }, false,
            555, 666, 777, 888, 999, false, 1111, 2222, "Device", true, 3333, 4444, new(2), new("id", "polyline", "sumaryPolyline"),
            new(new() { 1, 2, 3 }, new() { 4, 5, 6 }, new() { 7, 8, 9 }, new() { 10, 11, 12 }, new() { 13, 14, 15 }, new()));
    }
}
