using MediatR;

namespace Achievements.Application.Features.Achievements.Queries.GetAchievements;
public record GetAchievementsQuery() : IRequest<IEnumerable<GetAchievementsQueryResponse>>;
