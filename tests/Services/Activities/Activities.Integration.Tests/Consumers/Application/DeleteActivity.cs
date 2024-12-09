using Activities.Application.Tests.Common;
using Common.MessageBroker.Saga.DeleteActivity.Events;
using Common.MessageBroker.Saga.DeleteActivity.Messages;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;

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

        Assert.True(await Harness.Published
            .SelectAsync<ActivityDeletedEvent>(e =>
                e.Context.Message.CorrelationId == message.CorrelationId &&
                e.Context.Message.StravaActivityId == message.StravaActivityId &&
                e.Context.Message.StravaUserId == message.StravaUserId)
            .Any());
    }

    [Fact]
    public async Task ShouldDeleteActivity_ByGivenActivityId()
    {
        var activityId = 6;
        var activity = Aggregates.CreateActivity(activityId, 2);
        await Insert(activity);

        var activityToDelete = await Db.Activities
            .FirstOrDefaultAsync(e => e.StravaId == activityId);
        Assert.NotNull(activityToDelete);

        await Harness.Bus.Publish(new DeleteActivityMessage(Guid.NewGuid(), activityId, 2));
        Assert.True(await Harness.Published.Any<ActivityDeletedEvent>());
        await Task.Delay(50);


        var deletedActivity = await Db.Activities
            .FirstOrDefaultAsync(e => e.StravaId == activityId);

        Assert.Null(deletedActivity);
    }

    [Fact]
    public async Task ShouldNotDeleteActivity_IfActivityIdIsNotRelated()
    {
        var activityId = 6;
        var activity = Aggregates.CreateActivity(activityId, 2);
        await Insert(activity);


        var activityIdNotRelated = 7;
        await Harness.Bus.Publish(new DeleteActivityMessage(Guid.NewGuid(), activityIdNotRelated, 2));
        Assert.True(await Harness.Published.Any<ActivityDeletedEvent>());
        await Task.Delay(50);


        var notDeletedActivity = await Db.Activities
            .FirstOrDefaultAsync(e => e.StravaId == activityId);
        Assert.NotNull(notDeletedActivity);
    }
}
