using MediatR;

namespace Achievements.Application.Features.Achievements.Commands.DeleteAllUserAchievements;
public record DeleteAllUserAchievementsCommand(
    long StravaUserId) : IRequest<Unit>;
