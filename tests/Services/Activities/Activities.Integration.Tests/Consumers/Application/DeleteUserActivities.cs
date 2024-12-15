using Activities.Application.Tests.Common;
using Common.MessageBroker.Saga.DeleteAllUserdData.Events;
using Common.MessageBroker.Saga.DeleteAllUserdData.Messages;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;

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

    [Fact]
    public async Task ShouldDeleteAllUserActivitiesWithRelatedStreams_ByGivenUserId()
    {
        var userId = 56;
        var message = new DeleteUserActivitiesMessage(Guid.NewGuid(), userId);

        var activity = Aggregates.CreateActivity(1, userId);
        var stream = Aggregates.CreateStream(activity.Id);

        await Insert(activity);
        await Insert(stream);

        var activity2 = Aggregates.CreateActivity(2, userId);
        var stream2 = Aggregates.CreateStream(activity2.Id);
        await Insert(activity2);
        await Insert(stream2);

        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<UserActivitiesDeletedEvent>());
        await Task.Delay(50);

        var activities = await Db.Activities.ToListAsync();
        var streams = await Db.Streams.ToListAsync();

        Assert.Empty(activities);
        Assert.Empty(streams);
    }

    [Fact]
    public async Task ShouldNotDeleteAllUserActivitiesWithRelatedStreams_IfUserIdIsNotRelated()
    {
        var userId = 59;

        var activity = Aggregates.CreateActivity(1, userId);
        var stream = Aggregates.CreateStream(activity.Id);
        await Insert(activity);
        await Insert(stream);

        var activity2 = Aggregates.CreateActivity(2, userId);
        var stream2 = Aggregates.CreateStream(activity2.Id);
        await Insert(activity2);
        await Insert(stream2);

        var notRelatedUserId = 6;
        var message = new DeleteUserActivitiesMessage(Guid.NewGuid(), notRelatedUserId);
        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<UserActivitiesDeletedEvent>());
        await Task.Delay(50);

        var activities = await Db.Activities.ToListAsync();
        var streams = await Db.Streams.ToListAsync();

        Assert.Equal(2, activities.Count);
        Assert.Equal(2, streams.Count);
    }
}
