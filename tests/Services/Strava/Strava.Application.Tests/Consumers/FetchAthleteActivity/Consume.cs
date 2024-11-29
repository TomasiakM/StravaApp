using Common.Domain.Enums;
using Common.MessageBroker.Contracts.Activities;
using Common.MessageBroker.Saga.ProcessActivityData;
using Common.Tests.Utils;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Strava.Application.Consumers;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Application.Models;
using Strava.Contracts.Activity;

namespace Strava.Application.Tests.Consumers.FetchAthleteActivity;
public class Consume
{
    private readonly Mock<IUserActivityService> _userActivityServiceMock = new();
    private readonly Mock<IActivityStreamsService> _activityStreamsServiceMock = new();
    private readonly Mock<IBus> _busMock = new();

    [Fact]
    public async Task ShouldPublishProcessActivityDataMessage()
    {
        var activity = new StravaActivityDetailsResponse(1, "2", 3, 4, 5, 6, SportType.AlpineSki,
            new(), new(), new float[] { 0, 0 }, new float[] { 0, 0 }, true, 7, 8, 9, 10, 11,
            true, 12, 13, "14", true, 15, 16, new(17), new("18", "19", "20"));
        var streams = new ActivityStreams(new(), new(), new(), new(), new(), new());

        var message = new FetchAthleteActivityEvent(activity.Athlete.Id, activity.Id);

        var consumer = new FetchAthleteActivityEventConsumer(
            Mock.Of<ILogger<FetchAthleteActivityEventConsumer>>(),
            _userActivityServiceMock.Object,
            _activityStreamsServiceMock.Object,
            _busMock.Object,
            MapperFactory.Create(typeof(FetchAthleteActivityEventConsumer).Assembly));

        var consumeContext = Mock.Of<ConsumeContext<FetchAthleteActivityEvent>>(context =>
            context.Message == message);

        _userActivityServiceMock
            .Setup(e => e.GetAsync(message.StravaUserId, message.StravaActivityId, default))
            .ReturnsAsync(activity);

        _activityStreamsServiceMock
            .Setup(e => e.GetAsync(message.StravaUserId, message.StravaActivityId, default))
            .ReturnsAsync(streams);

        await consumer.Consume(consumeContext);

        _busMock.Verify(e => e.Publish(It.IsAny<ProcessActivityDataMessage>(), default), Times.Once);
    }

    [Fact]
    public async Task ShouldNotPublishProcessActivityDataMessage()
    {
        var message = new FetchAthleteActivityEvent(1, 2);

        var consumer = new FetchAthleteActivityEventConsumer(
            Mock.Of<ILogger<FetchAthleteActivityEventConsumer>>(),
            _userActivityServiceMock.Object,
            _activityStreamsServiceMock.Object,
            _busMock.Object,
            MapperFactory.Create(typeof(FetchAthleteActivityEventConsumer).Assembly));

        var consumeContext = Mock.Of<ConsumeContext<FetchAthleteActivityEvent>>(context =>
            context.Message == message);

        _userActivityServiceMock
            .Setup(e => e.GetAsync(message.StravaUserId, message.StravaActivityId, default))
            .ReturnsAsync((StravaActivityDetailsResponse?)null);

        await consumer.Consume(consumeContext);

        _busMock.Verify(e => e.Publish(It.IsAny<ProcessActivityDataMessage>(), default), Times.Never);
    }
}
