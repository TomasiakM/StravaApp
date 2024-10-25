using Athletes.Application.Interfaces;
using Athletes.Domain.Aggregates.Athletes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Athletes.Application.Features.Athletes.Commands.Create;
internal sealed class CreateAthleteCommandHandler : IRequestHandler<CreateAthleteCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAthleteCommandHandler> _logger;

    public CreateAthleteCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAthleteCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = AthleteAggregate.Create(
            request.Id,
            request.Username,
            request.Firstname,
            request.Lastname,
            request.Profile,
            request.ProfileMedium,
            request.CreatedAt);

        _unitOfWork.Athletes.Add(athlete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("New athlete:{AthleteId} created.", request.Id);

        return Unit.Value;
    }
}
