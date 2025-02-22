using Activities.Application.Features.Activities.Commands.DeleteAllUserActivities;
using Activities.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace Activities.Integration.Tests.Features.Commands;
public class DeleteAll : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public DeleteAll(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldDeleteAllUserActivitiesWithRelatedStreams_ByGivenUserId()
    {
        var userId = 56;
        var message = new DeleteAllUserActivitiesCommand(userId);

        var activity = Aggregates.CreateActivity(1, userId);
        var stream = Aggregates.CreateStream(activity.Id);
        await Insert(activity);
        await Insert(stream);

        var activity2 = Aggregates.CreateActivity(2, userId);
        var stream2 = Aggregates.CreateStream(activity2.Id);
        await Insert(activity2);
        await Insert(stream2);


        await Mediator.Send(message);


        var activities = await Db.Activities.ToListAsync();
        var streams = await Db.Streams.ToListAsync();
        Assert.Empty(activities);
        Assert.Empty(streams);
    }

    [Fact]
    public async Task ShouldNotDeleteAllUserActivitiesWithRelatedStreams_IfUserIdIsNotRelated()
    {
        var userId = 59;
        var notRelatedUserId = 6;
        var message = new DeleteAllUserActivitiesCommand(notRelatedUserId);

        var activity = Aggregates.CreateActivity(1, userId);
        var stream = Aggregates.CreateStream(activity.Id);
        await Insert(activity);
        await Insert(stream);

        var activity2 = Aggregates.CreateActivity(2, userId);
        var stream2 = Aggregates.CreateStream(activity2.Id);
        await Insert(activity2);
        await Insert(stream2);


        await Mediator.Send(message);


        var activities = await Db.Activities.ToListAsync();
        var streams = await Db.Streams.ToListAsync();
        Assert.Equal(2, activities.Count);
        Assert.Equal(2, streams.Count);
    }
}
