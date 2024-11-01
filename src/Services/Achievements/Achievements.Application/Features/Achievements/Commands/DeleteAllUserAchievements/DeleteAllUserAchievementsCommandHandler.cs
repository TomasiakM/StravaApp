using Achievements.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Achievements.Application.Features.Achievements.Commands.DeleteAllUserAchievements;
internal sealed class DeleteAllUserAchievementsCommandHandler : IRequestHandler<DeleteAllUserAchievementsCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAllUserAchievementsCommandHandler> _logger;

    public DeleteAllUserAchievementsCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAllUserAchievementsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteAllUserAchievementsCommand request, CancellationToken cancellationToken)
    {
        var achievements = await _unitOfWork.Achievements.GetAllAsync(
                a => a.StravaUserId == request.StravaUserId,
                cancellationToken: cancellationToken);

        _unitOfWork.Achievements.DeleteRange(achievements);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Users:{UserId} achievements deleted successfully.", request.StravaUserId);

        return Unit.Value;
    }
}
