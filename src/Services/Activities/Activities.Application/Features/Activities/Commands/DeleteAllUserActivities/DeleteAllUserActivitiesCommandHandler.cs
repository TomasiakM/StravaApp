using Activities.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Activities.Application.Features.Activities.Commands.DeleteAllUserActivities;
internal sealed class DeleteAllUserActivitiesCommandHandler : IRequestHandler<DeleteAllUserActivitiesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAllUserActivitiesCommandHandler> _logger;

    public DeleteAllUserActivitiesCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAllUserActivitiesCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteAllUserActivitiesCommand request, CancellationToken cancellationToken)
    {
        var activicities = await _unitOfWork.Activities.GetAllAsync(
            a => a.StravaUserId == request.StravaUserId,
            cancellationToken: cancellationToken);

        var activityIds = activicities.Select(e => e.Id);
        var streams = await _unitOfWork.Streams.GetAllAsync(
            s => activityIds.Contains(s.ActivityId),
            cancellationToken: cancellationToken);

        _unitOfWork.Activities.DeleteRange(activicities);
        _unitOfWork.Streams.DeleteRange(streams);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Users:{UserId} activities and streams deleted successfully.", request.StravaUserId);

        return Unit.Value;
    }
}
