using MediatR;

namespace Achievements.Application.Features.Achievements.Commands.UpdateAchievements;
public record UpdateAchievementsCommand(
    long StravaUserId) : IRequest<Unit>;
