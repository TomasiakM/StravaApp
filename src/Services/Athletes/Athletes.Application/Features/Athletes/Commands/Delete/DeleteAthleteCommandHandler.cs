using Athletes.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Athletes.Application.Features.Athletes.Commands.Delete;
internal sealed class DeleteAthleteCommandHandler : IRequestHandler<DeleteAthleteCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAthleteCommandHandler> _logger;

    public DeleteAthleteCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAthleteCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _unitOfWork.Athletes
            .GetAsync(e => e.StravaUserId == request.StravaUserId, cancellationToken: cancellationToken);

        if (athlete is not null)
        {
            _unitOfWork.Athletes.Delete(athlete);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Athlete:{AthleteId} deleted successfully.", athlete.StravaUserId);

            return Unit.Value;
        }

        _logger.LogWarning("Athlete:{AthleteId} not found.", request.StravaUserId);

        return Unit.Value;
    }
}
