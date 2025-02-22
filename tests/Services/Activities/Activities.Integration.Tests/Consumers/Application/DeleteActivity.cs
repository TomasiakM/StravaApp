using Common.MessageBroker.Saga.DeleteActivity.Events;
using Common.MessageBroker.Saga.DeleteActivity.Messages;
using MassTransit.Testing;

namespace Activities.Integration.Tests.Consumers.Application;
public class DeleteActivity : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public DeleteActivity(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldPublishCorrelatedEvent()
    {
        var message = new DeleteActivityMessage(Guid.NewGuid(), 1, 2);


        await Harness.Bus.Publish(message);


        Assert.True(await Harness.Consumed.Any<DeleteActivityMessage>());
        Assert.True(await Harness.Published
            .SelectAsync<ActivityDeletedEvent>(e =>
                e.Context.Message.CorrelationId == message.CorrelationId &&
                e.Context.Message.StravaActivityId == message.StravaActivityId &&
                e.Context.Message.StravaUserId == message.StravaUserId)
            .Any());
    }
}
