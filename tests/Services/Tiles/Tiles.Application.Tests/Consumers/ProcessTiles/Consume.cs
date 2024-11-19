using Common.Domain.Enums;
using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using Tiles.Application.Consumers;
using Tiles.Application.Features.ActivityTiles.Commands.Create;
using Tiles.Application.Features.ActivityTiles.Commands.Delete;
using Tiles.Application.Features.ActivityTiles.Commands.Update;
using Tiles.Application.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Tests.Consumers.ProcessTiles;
public class Consume
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBus> _busMock = new();
    private readonly Mock<ISender> _senderMock = new();

    [Theory]
    [MemberData(nameof(NotAllowedSportTypesCases))]
    public async Task ShouldSendDeleteTilesCommandWhenWhenSportTypeIsNotAllowedAndTilesExists(SportType sportType)
    {
        var correlationId = Guid.NewGuid();
        var activityId = 5;
        var userId = 6;
        var message = new ProcessTilesMessage(correlationId, activityId, userId, new(2022, 1, 1), sportType, new());

        var consumer = new ProcessTilesMessageConsumer(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<ProcessTilesMessageConsumer>>(),
            _busMock.Object,
            _senderMock.Object);
        var consumeContext = Mock.Of<ConsumeContext<ProcessTilesMessage>>(context =>
            context.Message == message);

        _unitOfWorkMock
            .Setup(e => e.Tiles.AnyAsync(It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(), default))
            .ReturnsAsync(true);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(new DeleteActivityTilesCommand(message.StravaActivityId), default), Times.Once);
        _busMock.Verify(e => e.Publish(new TilesProcessedEvent(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId), default), Times.Once);
    }

    [Theory]
    [MemberData(nameof(NotAllowedSportTypesCases))]
    public async Task ShouldNotSendDeleteTilesCommandWhenSportTypeIsNotAllowedAndTilesNotExists(SportType sportType)
    {
        var correlationId = Guid.NewGuid();
        var activityId = 5;
        var userId = 6;
        var message = new ProcessTilesMessage(correlationId, activityId, userId, new(2022, 1, 1), sportType, new());

        var consumer = new ProcessTilesMessageConsumer(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<ProcessTilesMessageConsumer>>(),
            _busMock.Object,
            _senderMock.Object);
        var consumeContext = Mock.Of<ConsumeContext<ProcessTilesMessage>>(context =>
            context.Message == message);

        _unitOfWorkMock
            .Setup(e => e.Tiles.AnyAsync(It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(), default))
            .ReturnsAsync(false);

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(It.IsAny<DeleteActivityTilesCommand>(), default), Times.Never);
        _busMock.Verify(e => e.Publish(new TilesProcessedEvent(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId), default), Times.Once);
    }

    [Theory]
    [MemberData(nameof(AllowedSportTypesCases))]
    public async Task ShouldSendCreateTilesCommandWhenTilesNotExistsAndRecalculationIsRequired(SportType sportType)
    {
        var correlationId = Guid.NewGuid();
        var activityId = 5;
        var userId = 6;
        var message = new ProcessTilesMessage(correlationId, activityId, userId, new(2022, 1, 1), sportType, new());

        var consumer = new ProcessTilesMessageConsumer(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<ProcessTilesMessageConsumer>>(),
            _busMock.Object,
            _senderMock.Object);
        var consumeContext = Mock.Of<ConsumeContext<ProcessTilesMessage>>(context =>
            context.Message == message);

        _unitOfWorkMock
            .Setup(e => e.Tiles.AnyAsync(It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(), default))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(e => e.Coordinates.GetAsync(
                It.IsAny<Expression<Func<CoordinatesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(CoordinatesAggregate.Create(activityId, new() { LatLng.Create(1, 1) }));

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(It.IsAny<CreateActivityTilesCommand>(), default), Times.Once);
        _busMock.Verify(e => e.Publish(new TilesProcessedEvent(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId), default), Times.Once);
    }

    [Theory]
    [MemberData(nameof(AllowedSportTypesCases))]
    public async Task ShouldSendUpdateTilesCommandWhenTilesExistsAndRecalculationIsRequired(SportType sportType)
    {
        var correlationId = Guid.NewGuid();
        var activityId = 5;
        var userId = 6;
        var message = new ProcessTilesMessage(correlationId, activityId, userId, new(2022, 1, 1), sportType, new());

        var consumer = new ProcessTilesMessageConsumer(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<ProcessTilesMessageConsumer>>(),
            _busMock.Object,
            _senderMock.Object);
        var consumeContext = Mock.Of<ConsumeContext<ProcessTilesMessage>>(context =>
            context.Message == message);

        _unitOfWorkMock
            .Setup(e => e.Tiles.AnyAsync(It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(), default))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(e => e.Coordinates.GetAsync(
                It.IsAny<Expression<Func<CoordinatesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(CoordinatesAggregate.Create(activityId, new() { LatLng.Create(1, 1) }));

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(It.IsAny<UpdateActivityTilesCommand>(), default), Times.Once);
        _busMock.Verify(e => e.Publish(new TilesProcessedEvent(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId), default), Times.Once);
    }

    [Theory]
    [MemberData(nameof(AllowedSportTypesCases))]
    public async Task ShouldDoNothingWhenCalculationIsNotRequired(SportType sportType)
    {
        var correlationId = Guid.NewGuid();
        var activityId = 5;
        var userId = 6;
        var message = new ProcessTilesMessage(correlationId, activityId, userId, new(2022, 1, 1), sportType, new());

        var consumer = new ProcessTilesMessageConsumer(
            _unitOfWorkMock.Object,
            Mock.Of<ILogger<ProcessTilesMessageConsumer>>(),
            _busMock.Object,
            _senderMock.Object);
        var consumeContext = Mock.Of<ConsumeContext<ProcessTilesMessage>>(context =>
            context.Message == message);

        _unitOfWorkMock
            .Setup(e => e.Tiles.AnyAsync(It.IsAny<Expression<Func<ActivityTilesAggregate, bool>>>(), default))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(e => e.Coordinates.GetAsync(
                It.IsAny<Expression<Func<CoordinatesAggregate, bool>>>(),
                default, default, default, default))
            .ReturnsAsync(CoordinatesAggregate.Create(activityId, new() { }));

        await consumer.Consume(consumeContext);

        _senderMock.Verify(e => e.Send(It.IsAny<CreateActivityTilesCommand>(), default), Times.Never);
        _senderMock.Verify(e => e.Send(It.IsAny<UpdateActivityTilesCommand>(), default), Times.Never);
        _senderMock.Verify(e => e.Send(It.IsAny<DeleteActivityTilesCommand>(), default), Times.Never);
        _busMock.Verify(e => e.Publish(new TilesProcessedEvent(
            message.CorrelationId,
            message.StravaActivityId,
            message.StravaUserId), default), Times.Once);
    }

    public static IEnumerable<object[]> NotAllowedSportTypesCases()
    {
        foreach (var sportType in GetNotAllowedSportTypes())
        {
            yield return new object[] { sportType };
        }
    }

    public static IEnumerable<object[]> AllowedSportTypesCases()
    {
        var notAllowed = GetNotAllowedSportTypes();

        foreach (SportType sportType in Enum.GetValues(typeof(SportType)))
        {
            if (notAllowed.Any(e => e == sportType)) continue;

            yield return new object[] { sportType };
        }
    }

    private static IEnumerable<SportType> GetNotAllowedSportTypes()
    {
        yield return SportType.EBikeRide;
        yield return SportType.EMountainBikeRide;
        yield return SportType.VirtualRide;
        yield return SportType.VirtualRow;
        yield return SportType.VirtualRun;
    }
}