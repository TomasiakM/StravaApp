using Activities.Application.Features.Activities.Commands.Update;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Enums;
using Common.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Activities.Integration.Tests.Features.Commands;
public class Update : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public Update(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldUpdateActivity_WhenActivityExists()
    {
        var activityId = 2;
        var userId = 4;
        var command = new UpdateActivityCommand(
            activityId, "Name2", 1000, 1100, 1200, 1300,
            SportType.Tennis, new(2022, 1, 3), new(2022, 1, 4),
            new double[] { 5, 6 }, new double[] { 7, 8 }, false, 1400, 1500,
            1600, 1700, 1800, false, 1900, 2000, "Device2", false, 2100, 2200,
            new(userId), new("2", "Polyline", "SummaryPolyline"),
            new(new() { 1, 2 }, new() { 3, 4 }, new() { 5, 6 }, new() { 7, 8 }, new() { 9, 10 },
            new() { LatLng.Create(1, 2), LatLng.Create(3, 4) }));

        var activity = ActivityAggregate.Create(activityId, userId, "Name", "Device", SportType.Badminton, true, 100, 110, 120, 130, 140,
            Speed.Create(150, 160), Time.Create(170, 180, new(2022, 1, 1), new(2022, 1, 2)),
            Watts.Create(true, 190, 200), Heartrate.Create(true, 210, 220),
            Map.Create(LatLng.Create(1, 2), LatLng.Create(3, 4), "Poly", "SummaryPoly"));
        var stream = StreamAggregate.Create(activity.Id, new(), new(), new(), new(), new());
        await Insert(activity);
        await Insert(stream);


        await Mediator.Send(command);


        var updatedActivity = await Db.Activities
            .FirstOrDefaultAsync(e => e.StravaId == activityId);
        Assert.NotNull(updatedActivity);
        var updatedStream = await Db.Streams
            .FirstOrDefaultAsync(e => e.ActivityId == updatedActivity.Id);
        Assert.NotNull(updatedStream);

        Assert.Equal(command.Name, updatedActivity.Name);
        Assert.Equal(command.Streams.LatLngs.Count, updatedStream.LatLngs.Count);
    }
}
