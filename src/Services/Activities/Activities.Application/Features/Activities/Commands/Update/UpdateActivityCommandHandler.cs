using Activities.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Features.Activities.Commands.Update;
internal sealed class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityAggregateFactory _activityAggregateFactory;
    private readonly ILogger<UpdateActivityCommandHandler> _logger;

    public UpdateActivityCommandHandler(IUnitOfWork unitOfWork, IActivityAggregateFactory activityAggregateFactory, ILogger<UpdateActivityCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _activityAggregateFactory = activityAggregateFactory;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities.GetAsync(e => e.StravaId == request.Id, cancellationToken: cancellationToken);
        if (activity is null)
        {
            throw new Exception("Activity not found.");
        }

        activity.Update(
            request.Name,
            request.DeviceName,
            request.SportType,
            request.Private,
            request.Distance,
            request.TotalElevationGain,
            request.AverageCadence,
            request.Kilojoules,
            request.Calories,
            _activityAggregateFactory.CreateSpeed(request),
            _activityAggregateFactory.CreateTime(request),
            _activityAggregateFactory.CreateWatts(request),
            _activityAggregateFactory.CreateHeartrate(request),
            _activityAggregateFactory.CreateMap(request));

        var streams = await _unitOfWork.Streams.GetAsync(e => e.ActivityId == activity.Id, cancellationToken: cancellationToken);
        if (streams is null)
        {
            throw new Exception("Streams not found.");
        }

        streams.Update(
            request.Streams.Cadence,
            request.Streams.Heartrate,
            request.Streams.Altitude,
            request.Streams.Distance,
            request.Streams.LatLngs);

        _unitOfWork.Streams.Update(streams);
        _unitOfWork.Activities.Update(activity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Activity:{ActivityId} updated successfully.", request.Id);

        return Unit.Value;
    }
}
