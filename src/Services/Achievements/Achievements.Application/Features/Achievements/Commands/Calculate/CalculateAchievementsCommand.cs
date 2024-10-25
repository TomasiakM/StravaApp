using MediatR;

namespace Achievements.Application.Features.Achievements.Commands.Calculate;
public record CalculateAchievementsCommand(
    long StravaUserId) : IRequest<Unit>;
