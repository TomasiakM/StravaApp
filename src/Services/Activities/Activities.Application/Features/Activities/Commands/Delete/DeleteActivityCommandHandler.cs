using Activities.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Features.Activities.Commands.Delete;
internal sealed class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteActivityCommandHandler> _logger;

    public DeleteActivityCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteActivityCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities.GetAsync(e => e.StravaId == request.StravaActivityId, cancellationToken: cancellationToken);
        if (activity is not null)
        {
            var streams = await _unitOfWork.Streams
                .GetAsync(e => e.ActivityId == activity.Id, cancellationToken: cancellationToken);

            if (streams is not null)
            {
                _unitOfWork.Streams.Delete(streams);
            }

            _unitOfWork.Activities.Delete(activity);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Activity:{ActivityId} has been removed successfully.", request.StravaActivityId);

        return Unit.Value;
    }
}
