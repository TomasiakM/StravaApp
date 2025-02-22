using Activities.Application.Features.Activities.Commands.Delete;
using Activities.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace Activities.Integration.Tests.Features.Commands;
public class Delete : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public Delete(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldDeleteActivity_ByGivenActivityId()
    {
        var activityId = 6;
        var command = new DeleteActivityCommand(activityId);

        var activity = Aggregates.CreateActivity(activityId, 2);
        await Insert(activity);


        await Mediator.Send(command);


        var deletedActivity = await Db.Activities
            .FirstOrDefaultAsync(e => e.StravaId == activityId);
        Assert.Null(deletedActivity);
    }

    [Fact]
    public async Task ShouldNotDeleteActivity_IfActivityIdIsNotRelated()
    {
        var activityId = 6;
        var otherActivityid = 7;
        var command = new DeleteActivityCommand(otherActivityid);

        var activity = Aggregates.CreateActivity(activityId, 2);
        await Insert(activity);


        await Mediator.Send(command);


        var notDeletedActivity = await Db.Activities
            .FirstOrDefaultAsync(e => e.StravaId == activityId);
        Assert.NotNull(notDeletedActivity);
    }
}
