
using Common.MessageBroker.Contracts.Activities;
using Common.MessageBroker.Contracts.Athletes;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Strava.Application.Features.StravaHook.HandleEvent;
internal sealed class HandleEventCommandHandler : IRequestHandler<HandleEventCommand, Unit>
{
    private readonly ILogger<HandleEventCommandHandler> _logger;
    private readonly IBus _bus;

    public HandleEventCommandHandler(ILogger<HandleEventCommandHandler> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task<Unit> Handle(HandleEventCommand request, CancellationToken cancellationToken)
    {
        if (request.ObjectType == ObjectTypeCommand.Activity)
        {
            if (request.AspectType == AspectTypeCommand.Create || request.AspectType == AspectTypeCommand.Update)
            {
                await _bus.Publish(new FetchAthleteActivityEvent(request.OwnerId, request.ObjectId));

                return Unit.Value;
            }
            else
            {
                await _bus.Publish(new DeleteActivityEvent(request.ObjectId));

                return Unit.Value;
            }
        }

        if (request.ObjectType == ObjectTypeCommand.Athlete)
        {
            if (request.Updates.Authorized == "false")
            {
                await _bus.Publish(new UnauthorizeAthleteEvent(request.OwnerId));

                return Unit.Value;
            }
        }

        _logger.LogWarning("Event data was not processed: {Request}.", request);
        return Unit.Value;
    }
}
