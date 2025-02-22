using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit.Testing;

namespace Activities.Integration.Tests.Consumers.Application;
public class DeleteUserActivities : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public DeleteUserActivities(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldPublishCorrelatedEvent()
    {
        var message = new DeleteUserActivitiesMessage(Guid.NewGuid(), 1);


        await Harness.Bus.Publish(message);


        Assert.True(await Harness.Published
            .SelectAsync<UserActivitiesDeletedEvent>(e =>
                e.Context.Message.CorrelationId == message.CorrelationId &&
                e.Context.Message.StravaUserId == message.StravaUserId)
            .Any());
    }
}
