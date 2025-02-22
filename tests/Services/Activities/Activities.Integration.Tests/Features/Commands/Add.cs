using Activities.Application.Features.Activities.Commands.Add;
using Common.Domain.Enums;
using Common.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Activities.Integration.Tests.Features.Commands;
public class Add : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public Add(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldCreateNewActivity_WhenActivityNotExists()
    {
        var command = new AddActivityCommand(
            1, "Name", 100, 110, 120, 130,
            SportType.Badminton, new(2022, 1, 1), new(2022, 1, 2),
            new double[] { 1, 1 }, new double[] { 2, 2 }, true, 140, 150,
            160, 170, 180, true, 190, 200, "Device", true, 210, 220,
            new(1), new("2", "Polyline", "SummaryPolyline"),
            new(new() { 1, 2 }, new() { 3, 4 }, new() { 5, 6 }, new() { 7, 8 },
            new() { 9, 10 }, new() { LatLng.Create(1, 2), LatLng.Create(2, 3) }));


        await Mediator.Send(command);


        var activity = await Db.Activities.FirstOrDefaultAsync(e => e.StravaId == command.Id);
        Assert.NotNull(activity);
        var stream = await Db.Streams.FirstOrDefaultAsync(e => e.ActivityId == activity.Id);
        Assert.NotNull(stream);
    }
}
