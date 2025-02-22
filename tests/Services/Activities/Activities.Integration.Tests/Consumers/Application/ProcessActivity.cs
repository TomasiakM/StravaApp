using Common.Domain.Enums;
using Common.Domain.Models;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MassTransit.Testing;

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
}
