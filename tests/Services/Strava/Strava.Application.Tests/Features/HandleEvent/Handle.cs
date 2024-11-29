using Common.MessageBroker.Contracts.Activities;
using Common.MessageBroker.Saga.DeleteActivity;
using Common.MessageBroker.Saga.DeleteAllUserdData;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Strava.Application.Features.StravaHook.HandleEvent;

namespace Strava.Application.Tests.Features.HandleEvent;
public class Handle
{
    private readonly Mock<IBus> _busMock = new();

    [Theory]
    [InlineData(AspectTypeCommand.Create)]
    [InlineData(AspectTypeCommand.Update)]
    public async Task ShouldPublishFetchAthleteActivityMessage(AspectTypeCommand type)
    {
        var command = new HandleEventCommand(ObjectTypeCommand.Activity, 1, type,
            1, 1, 1, new(null, null, null, null));

        var handler = new HandleEventCommandHandler(
            Mock.Of<ILogger<HandleEventCommandHandler>>(),
            _busMock.Object);

        await handler.Handle(command, default);

        _busMock.Verify(e => e.Publish(It.IsAny<FetchAthleteActivityEvent>(), default), Times.Once);
    }

    [Fact]
    public async Task ShouldPublishDeleteActivityDataMessage()
    {
        var command = new HandleEventCommand(ObjectTypeCommand.Activity, 1, AspectTypeCommand.Delete,
            1, 1, 1, new(null, null, null, null));

        var handler = new HandleEventCommandHandler(
            Mock.Of<ILogger<HandleEventCommandHandler>>(),
            _busMock.Object);

        await handler.Handle(command, default);

        _busMock.Verify(e => e.Publish(It.IsAny<DeleteActivityDataMessage>(), default), Times.Once);
    }

    [Fact]
    public async Task ShouldPublishDeleteAllUserDataStartSagaMessage()
    {
        var command = new HandleEventCommand(ObjectTypeCommand.Athlete, 1, AspectTypeCommand.Update,
            1, 1, 1, new(null, null, null, "false"));

        var handler = new HandleEventCommandHandler(
            Mock.Of<ILogger<HandleEventCommandHandler>>(),
            _busMock.Object);

        await handler.Handle(command, default);

        _busMock.Verify(e => e.Publish(It.IsAny<DeleteAllUserDataStartSagaMessage>(), default), Times.Once);
    }

    [Fact]
    public async Task ShouldNotPublishDeleteAllUserDataStartSagaMessage()
    {
        var command = new HandleEventCommand(ObjectTypeCommand.Athlete, 1, AspectTypeCommand.Update,
            1, 1, 1, new(null, null, null, "true"));

        var handler = new HandleEventCommandHandler(
            Mock.Of<ILogger<HandleEventCommandHandler>>(),
            _busMock.Object);

        await handler.Handle(command, default);

        _busMock.Verify(e => e.Publish(It.IsAny<DeleteAllUserDataStartSagaMessage>(), default), Times.Never);
    }
}
