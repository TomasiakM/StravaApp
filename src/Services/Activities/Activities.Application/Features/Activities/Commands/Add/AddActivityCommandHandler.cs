using Activities.Application.Interfaces;
using Activities.Domain.Aggregates.Streams;
using MediatR;

namespace Activities.Application.Features.Activities.Commands.Add;
internal sealed class AddActivityCommandHandler : IRequestHandler<AddActivityCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityAggregateFactory _activityAggregateFactory;

    public AddActivityCommandHandler(IUnitOfWork unitOfWork, IActivityAggregateFactory activityAggregateFactory)
    {
        _unitOfWork = unitOfWork;
        _activityAggregateFactory = activityAggregateFactory;
    }

    public async Task<Unit> Handle(AddActivityCommand request, CancellationToken cancellationToken)
    {
        var activity = _activityAggregateFactory.CreateActivity(request);
        var streams = StreamAggregate.Create(
            activity.Id,
            request.Streams.Cadence,
            request.Streams.Heartrate,
            request.Streams.Altitude,
            request.Streams.Distance,
            request.Streams.LatLngs);

        _unitOfWork.Activities.Add(activity);
        _unitOfWork.Streams.Add(streams);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
