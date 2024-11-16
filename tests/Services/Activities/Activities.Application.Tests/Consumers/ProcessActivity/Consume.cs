using Activities.Application.Consumers;
using Activities.Application.Features.Activities.Commands.Add;
using Activities.Application.Features.Activities.Commands.Update;
using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Common.Domain.Enums;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using Common.Tests.Utils;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Activities.Application.Tests.Consumers.ProcessActivity;
public class Consume
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly Mock<IBus> _busMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldConsumeProcessActivityMessage(Guid correlationId, long stravaActivityId, long stravaUserId, bool isActivityCreated)
    {
        var mapper = MapperFactory.Create(typeof(ProcessActivityMessageConsumer).Assembly);
        var message = new ProcessActivityMessage(correlationId, stravaActivityId, "Test", 111, 222, 333, 444, SportType.Golf,
            new DateTime(1111111111), new DateTime(2222222222), new double[] { 1, 1 }, new double[] { 2, 2 }, false,
            555, 666, 777, 888, 999, false, 1111, 2222, "Device", true, 3333, 4444, new(stravaUserId), new("id", "polyline", "sumaryPolyline"),
            new(new() { 1, 2, 3 }, new() { 4, 5, 6 }, new() { 7, 8, 9 }, new() { 10, 11, 12 }, new() { 13, 14, 15 }, new()));

        _unitOfWorkMock
            .Setup(e => e.Activities.AnyAsync(
                It.IsAny<Expression<Func<ActivityAggregate, bool>>>(),
                default))
            .ReturnsAsync(isActivityCreated);

        var consumer = new ProcessActivityMessageConsumer(
            _busMock.Object,
            _senderMock.Object,
            mapper,
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<ProcessActivityMessageConsumer>>());

        var consumeContext = Mock.Of<ConsumeContext<ProcessActivityMessage>>(context =>
            context.Message == message);
        await consumer.Consume(consumeContext);

        _unitOfWorkMock.VerifyAll();

        _busMock.Verify(e => e.Publish(
            new ActivityProcessedEvent(
                message.CorrelationId,
                message.Id,
                message.Athlete.Id,
                message.StartDate,
                message.SportType,
                message.Streams.LatLngs),
            default),
            Times.Once);

        if (isActivityCreated)
        {
            _senderMock.Verify(e => e.Send(It.IsAny<UpdateActivityCommand>(), default), Times.Once);
        }
        else
        {
            _senderMock.Verify(e => e.Send(It.IsAny<AddActivityCommand>(), default), Times.Once);
        }
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[] { new Guid("5640a1b4-7c5f-4cd6-901f-c8c555bd3003"), 5, 65, true };
        yield return new object[] { new Guid("2154326e-2dd9-4d4e-8479-85f8ab7a0170"), 24, 41, true };
        yield return new object[] { new Guid("482220b5-c772-4aa2-8c43-0ad0bc2ca376"), 46, 23, false };
        yield return new object[] { new Guid("f93eb168-58f7-430e-8dfc-030858a4cd7c"), 215, 6, false };
    }
}
