using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Enums;
using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;

namespace Activities.Integration.Tests.Consumers.Application;
public class ProcessActivity : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public ProcessActivity(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldConsumeAndPublishCorrelatedEvent()
    {
        var message = new ProcessActivityMessage(
            Guid.NewGuid(), 1, "Name", 100, 110, 120, 130,
            SportType.Badminton, new(2022, 1, 1), new(2022, 1, 2),
            new double[] { 1, 1 }, new double[] { 2, 2 }, true, 140, 150,
            160, 170, 180, true, 190, 200, "Device", true, 210, 220,
            new(1), new("2", "Polyline", "SummaryPolyline"),
            new(new(), new(), new(), new(), new(), new() { LatLng.Create(1, 2), LatLng.Create(2, 3) }));


        await Harness.Bus.Publish(message);


        Assert.True(await Harness.Published
            .SelectAsync<ActivityProcessedEvent>(e =>
                e.Context.Message.CorrelationId == message.CorrelationId &&
                e.Context.Message.StravaActivityId == message.Id &&
                e.Context.Message.StravaUserId == message.Athlete.Id &&
                e.Context.Message.CreatedAt == message.StartDate &&
                e.Context.Message.SportType == message.SportType &&
                e.Context.Message.LatLngs.Count == message.Streams.LatLngs.Count &&
                Enumerable.SequenceEqual(e.Context.Message.LatLngs, message.Streams.LatLngs))
            .Any());
    }

    [Fact]
    public async Task ShouldCreateNewActivity_WhenActivityNotExists()
    {
        var message = new ProcessActivityMessage(
            Guid.NewGuid(), 1, "Name", 100, 110, 120, 130,
            SportType.Badminton, new(2022, 1, 1), new(2022, 1, 2),
            new double[] { 1, 1 }, new double[] { 2, 2 }, true, 140, 150,
            160, 170, 180, true, 190, 200, "Device", true, 210, 220,
            new(1), new("2", "Polyline", "SummaryPolyline"),
            new(new() { 1, 2 }, new() { 3, 4 }, new() { 5, 6 }, new() { 7, 8 },
            new() { 9, 10 }, new() { LatLng.Create(1, 2), LatLng.Create(2, 3) }));


        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<ActivityProcessedEvent>());
        await Task.Delay(50);


        var activity = await Db.Activities.FirstOrDefaultAsync(e => e.StravaId == message.Id);
        Assert.NotNull(activity);
        var stream = await Db.Streams.FirstOrDefaultAsync(e => e.ActivityId == activity.Id);
        Assert.NotNull(stream);
    }

    [Fact]
    public async Task ShouldUpdateActivity_WhenActivityExists()
    {
        var activityId = 2;
        var userId = 4;
        var message = new ProcessActivityMessage(
            Guid.NewGuid(), activityId, "Name2", 1000, 1100, 1200, 1300,
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


        await Harness.Bus.Publish(message);
        Assert.True(await Harness.Published.Any<ActivityProcessedEvent>());
        await Task.Delay(50);


        var updatedActivity = await Db.Activities
            .FirstOrDefaultAsync(e => e.StravaId == activityId);
        Assert.NotNull(updatedActivity);
        var updatedStream = await Db.Streams
            .FirstOrDefaultAsync(e => e.ActivityId == updatedActivity.Id);
        Assert.NotNull(updatedStream);

        Assert.Equal(message.Name, updatedActivity.Name);
        Assert.Equal(message.Streams.LatLngs.Count, updatedStream.LatLngs.Count);
    }
}
