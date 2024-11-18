using Athletes.Application.Consumers;
using Athletes.Application.Features.Athletes.Commands.Create;
using Athletes.Application.Features.Athletes.Commands.Update;
using Athletes.Application.Interfaces;
using Athletes.Domain.Aggregates.Athletes;
using Common.MessageBroker.Contracts.Athletes;
using MassTransit;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace Athletes.Application.Tests.Consumers.ReceivedAthleteData;
public class Consume
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ISender> _senderMock = new();

    [Fact]
    public async Task ShouldConsumeMessageAndSendCreateAthleteCommandWhenAthleteNotExists()
    {
        var message = new ReceivedAthleteDataEvent(1, "test", "first", "last", new DateTime(2022, 1, 1), "profileMedium", "profile");

        var consumer = new ReceivedAthleteDataEventConsumer(_unitOfWorkMock.Object, _senderMock.Object);
        var consumeContext = Mock.Of<ConsumeContext<ReceivedAthleteDataEvent>>(context =>
            context.Message == message);

        _unitOfWorkMock
            .Setup(e => e.Athletes.AnyAsync(It.IsAny<Expression<Func<AthleteAggregate, bool>>>(), default))
            .ReturnsAsync(false);

        await consumer.Consume(consumeContext);

        _unitOfWorkMock.Verify(e => e.Athletes.AnyAsync(
            It.IsAny<Expression<Func<AthleteAggregate, bool>>>(), default));

        _senderMock.Verify(e => e.Send(It.IsAny<CreateAthleteCommand>(), default), Times.Once);
    }

    [Fact]
    public async Task ShouldConsumeMessageAndSendUpdateAthleteCommandWhenAthleteExists()
    {
        var message = new ReceivedAthleteDataEvent(1, "test", "first", "last", new DateTime(2022, 1, 1), "profileMedium", "profile");

        var consumer = new ReceivedAthleteDataEventConsumer(_unitOfWorkMock.Object, _senderMock.Object);
        var consumeContext = Mock.Of<ConsumeContext<ReceivedAthleteDataEvent>>(context =>
            context.Message == message);

        _unitOfWorkMock
            .Setup(e => e.Athletes.AnyAsync(It.IsAny<Expression<Func<AthleteAggregate, bool>>>(), default))
            .ReturnsAsync(true);

        await consumer.Consume(consumeContext);

        _unitOfWorkMock.Verify(e => e.Athletes.AnyAsync(
            It.IsAny<Expression<Func<AthleteAggregate, bool>>>(), default));

        _senderMock.Verify(e => e.Send(It.IsAny<UpdateAthleteCommand>(), default), Times.Once);
    }
}
