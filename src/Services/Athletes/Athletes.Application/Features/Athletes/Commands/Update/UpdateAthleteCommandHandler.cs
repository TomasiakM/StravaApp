using Athletes.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Athletes.Application.Features.Athletes.Commands.Update;
internal sealed class UpdateAthleteCommandHandler : IRequestHandler<UpdateAthleteCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateAthleteCommandHandler> _logger;

    public UpdateAthleteCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateAthleteCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _unitOfWork.Athletes.GetAsync(e => e.StravaUserId == request.Id, cancellationToken: cancellationToken);

        if (athlete is null)
        {
            throw new Exception($"Athlete:{request.Id} not found, cannot update his data");
        }

        athlete.Update(
            request.Username,
            request.Firstname,
            request.Lastname,
            request.Profile,
            request.ProfileMedium);

        _unitOfWork.Athletes.Update(athlete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Athlete:{AthleteId} successfully updated.", request.Id);

        return Unit.Value;
    }
}
