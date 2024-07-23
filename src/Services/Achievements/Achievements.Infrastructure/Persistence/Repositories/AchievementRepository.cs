using Achievements.Domain.Aggregates.Achievement;
using Achievements.Domain.Aggregates.Achievement.ValueObjects;
using Common.Infrastructure.Persistence;

namespace Achievements.Infrastructure.Persistence.Repositories;
internal sealed class AchievementRepository
    : GenericRepository<Achievement, AchievementId>, IAchievementRepository
{
    public AchievementRepository(ServiceDbContext dbContext)
        : base(dbContext)
    {
    }
}
