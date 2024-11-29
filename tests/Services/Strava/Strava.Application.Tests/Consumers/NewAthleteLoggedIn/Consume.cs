using Common.MessageBroker.Contracts.Activities;
using Common.MessageBroker.Contracts.Athletes;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Strava.Application.Consumers;
using Strava.Application.Interfaces.Services.StravaDataServices;
using Strava.Contracts.Activity;

namespace Strava.Application.Tests.Consumers.NewAthleteLoggedIn;
public class Consume
{
    private readonly Mock<IAllUserActivitiesService> _allUserActivitiesServiceMock = new();
    private readonly Mock<IBus> _busMock = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ShouldPublishFetchAthleteActivityEventForAllActivities(List<StravaActivitySummaryResponse> activities)
    {
        var message = new NewAthleteLoggedInEvent(1);

        var consumer = new NewAthleteLoggedInEventConsumer(
            Mock.Of<ILogger<NewAthleteLoggedInEventConsumer>>(),
            _allUserActivitiesServiceMock.Object,
            _busMock.Object);

        var consumeContext = Mock.Of<ConsumeContext<NewAthleteLoggedInEvent>>(context =>
            context.Message == message);

        _allUserActivitiesServiceMock
            .Setup(e => e.GetAsync(message.StravaUserId, default))
            .ReturnsAsync(activities);

        await consumer.Consume(consumeContext);

        _busMock.Verify(e => e.Publish(It.IsAny<FetchAthleteActivityEvent>(), default), Times.Exactly(activities.Count));
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[] { new List<StravaActivitySummaryResponse>() };

        yield return new object[] { new List<StravaActivitySummaryResponse>
        {
            new(1, new(), new(2)),
        }};

        yield return new object[] { new List<StravaActivitySummaryResponse>
        {
            new(1, new(), new(2)),
            new(2, new(), new(2)),
            new(3, new(), new(2)),
            new(4, new(), new(2))
        }};
    }
}
