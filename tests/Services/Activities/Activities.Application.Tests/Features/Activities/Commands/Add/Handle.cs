using Activities.Application.Factories;
using Activities.Application.Features.Activities.Commands.Add;
using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Enums;
using Moq;

namespace Activities.Application.Tests.Features.Activities.Commands.Add;
public class Handle
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    [Fact]
    public async Task ShouldCreateActivity()
    {
        var command = CreateAddActivityCommand();
        var handler = new AddActivityCommandHandler(_unitOfWorkMock.Object, new ActivityAggregateFactory());

        _unitOfWorkMock.Setup(e => e.Activities).Returns(new Mock<IActivityRepository>().Object);
        _unitOfWorkMock.Setup(e => e.Streams).Returns(new Mock<IStreamRepository>().Object);

        await handler.Handle(command, default);

        _unitOfWorkMock.Verify(e => e.Activities.Add(It.IsAny<ActivityAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.Streams.Add(It.IsAny<StreamAggregate>()), Times.Once);
        _unitOfWorkMock.Verify(e => e.SaveChangesAsync(default), Times.Once);
    }

    public static AddActivityCommand CreateAddActivityCommand()
    {
        return new AddActivityCommand(1, "Test", 111, 222, 333, 444, SportType.Golf,
            new DateTime(1111111111), new DateTime(2222222222), new double[] { 1, 1 }, new double[] { 2, 2 }, false,
            555, 666, 777, 888, 999, false, 1111, 2222, "Device", true, 3333, 4444, new(2), new("id", "polyline", "sumaryPolyline"),
            new(new() { 1, 2, 3 }, new() { 4, 5, 6 }, new() { 7, 8, 9 }, new() { 10, 11, 12 }, new() { 13, 14, 15 }, new()));
    }
}
