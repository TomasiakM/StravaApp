﻿using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Activities.ValueObjects;
using Activities.Domain.Aggregates.Streams;
using Common.MessageBroker.Contracts.Activities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class ReceivedActivityDataEventConsumer
    : IConsumer<ReceivedActivityDataEvent>
{
    private readonly ILogger<ReceivedActivityDataEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityAggregateFactory _activityAggregateFactory;
    private readonly IBus _bus;

    public ReceivedActivityDataEventConsumer(ILogger<ReceivedActivityDataEventConsumer> logger, IUnitOfWork unitOfWork, IActivityAggregateFactory activityAggregateFactory, IBus bus)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _activityAggregateFactory = activityAggregateFactory;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<ReceivedActivityDataEvent> context)
    {
        _logger.LogInformation("Starting processing activity:{Id}.", context.Message.Id);

        var activity = await CreateOrUpdateActivity(context);
        await CreateOrUpdateStreams(activity.Id, context);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Processing activity:{Id} completed.", context.Message.Id);

        // add saga pattern to handle tiles and achievements services
    }

    private async Task<StreamAggregate> CreateOrUpdateStreams(ActivityId activityId, ConsumeContext<ReceivedActivityDataEvent> context)
    {
        var streams = await _unitOfWork.Streams
                    .GetAsync(e => e.ActivityId == activityId);

        if (streams is null)
        {
            streams = StreamAggregate.Create(
                activityId,
                context.Message.Streams.Cadence,
                context.Message.Streams.Heartrate,
                context.Message.Streams.Altitude,
                context.Message.Streams.Distance,
                context.Message.Streams.LatLngs);

            _unitOfWork.Streams.Add(streams);
        }
        else
        {
            streams.Update(
                context.Message.Streams.Cadence,
                context.Message.Streams.Heartrate,
                context.Message.Streams.Altitude,
                context.Message.Streams.Distance,
                context.Message.Streams.LatLngs);
        }

        return streams!;
    }

    private async Task<ActivityAggregate> CreateOrUpdateActivity(ConsumeContext<ReceivedActivityDataEvent> context)
    {
        var activity = await _unitOfWork.Activities
                    .GetAsync(e => e.StravaId == context.Message.Id);

        if (activity is null)
        {
            activity = _activityAggregateFactory.CreateActivity(context.Message);
            _unitOfWork.Activities.Add(activity);
        }
        else
        {
            var message = context.Message;
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
                    _activityAggregateFactory.CreateSpeed(message),
                    _activityAggregateFactory.CreateTime(message),
                    _activityAggregateFactory.CreateWatts(message),
                    _activityAggregateFactory.CreateHeartrate(message),
                    _activityAggregateFactory.CreateMap(message));
        }

        return activity;
    }
}
