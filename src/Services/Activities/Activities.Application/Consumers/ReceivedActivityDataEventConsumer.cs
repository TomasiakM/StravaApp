using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Common.Domain.Models;
using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class ReceivedActivityDataEventConsumer
    : IConsumer<ReceivedActivityDataEvent>
{
    private readonly ILogger<ReceivedActivityDataEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ReceivedActivityDataEventConsumer(ILogger<ReceivedActivityDataEventConsumer> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ReceivedActivityDataEvent> context)
    {
        _logger.LogInformation("Starting processing activity:{Id} data.", context.Message.Id);

        var message = context.Message;

        var speed = Speed.Create(message.MaxSpeed, message.AverageSpeed);
        var time = Time.Create(message.MovingTime, message.ElapsedTime, message.StartDate, message.StartDateLocal);
        var watts = Watts.Create(message.DeviceWatts, message.MaxWatts, message.AverageWatts);
        var heartrate = Heartrate.Create(message.HasHeartrate, message.MaxHeartrate, message.AverageHeartrate);

        var startLatLng = message.StartLatlng.Length == 2 ? LatLng.Create(message.StartLatlng[0], message.StartLatlng[1]) : null;
        var endLatLng = message.EndLatlng.Length == 2 ? LatLng.Create(message.EndLatlng[0], message.EndLatlng[1]) : null;
        var map = Map.Create(startLatLng, endLatLng, message.Map.Polyline, message.Map.SummaryPolyline);

        var activity = await _unitOfWork.Activities
            .GetAsync(e => e.StravaId == message.Id);

        if (activity is null)
        {
            activity = ActivityAggregate.Create(
                message.Id,
                message.Athlete.Id,
                message.Name,
                message.DeviceName,
                message.SportType,
                message.Private,
                message.Distance,
                message.TotalElevationGain,
                message.AverageCadence,
                message.Kilojoules,
                message.Calories,
                speed,
                time,
                watts,
                heartrate,
                map);

            _unitOfWork.Activities.Add(activity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("New activity:{Id} \"{Name}\" is created", activity.StravaId, activity.Name);
            return;
        }

        activity.Update(
            message.Name,
            message.DeviceName,
            message.SportType,
            message.Private,
            message.Distance,
            message.TotalElevationGain,
            message.AverageCadence,
            message.Kilojoules,
            message.Calories,
            speed,
            time,
            watts,
            heartrate,
            map);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Activity:{Id} \"{Name}\" updated successfully.", activity.StravaId, activity.Name);
    }
}
