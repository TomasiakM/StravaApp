using Activities.Application.Features.Activities.Commands.Add;
using Activities.Application.Features.Activities.Commands.Update;
using Activities.Application.Interfaces;
using Common.MessageBroker.Saga.ProcessActivityData.Events;
using Common.MessageBroker.Saga.ProcessActivityData.Messages;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Consumers;
public sealed class ProcessActivityMessageConsumer
    : IConsumer<ProcessActivityMessage>
{
    private readonly IBus _bus;
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProcessActivityMessageConsumer> _logger;

    public ProcessActivityMessageConsumer(IBus bus, ISender sender, IMapper mapper, IUnitOfWork unitOfWork, ILogger<ProcessActivityMessageConsumer> logger)
    {
        _bus = bus;
        _sender = sender;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessActivityMessage> context)
    {
        _logger.LogInformation("Starting processing activity:{Id}.", context.Message.Id);
        var isActivityCreated = await _unitOfWork.Activities.AnyAsync(e => e.StravaId == context.Message.Id);
        if (isActivityCreated)
        {
            await _sender.Send(_mapper.Map<UpdateActivityCommand>(context.Message));
        }
        else
        {
            await _sender.Send(_mapper.Map<AddActivityCommand>(context.Message));
        }

        _logger.LogInformation("[BUS]: Publishing {Event}.", nameof(ActivityProcessedEvent));
        await _bus.Publish(new ActivityProcessedEvent(
            context.Message.CorrelationId,
            context.Message.Id,
            context.Message.Athlete.Id,
            context.Message.StartDate,
            context.Message.SportType,
            context.Message.Streams.LatLngs));
    }
}
