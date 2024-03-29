using Athletes.Domain.Aggregates.Athletes;
using Athletes.Domain.Aggregates.Athletes.ValueObjects;
using Common.Infrastructure.Persistence;

namespace Athletes.Infrastructure.Persistence.Repositories;
internal sealed class AthleteRepository
    : GenericRepository<AthleteAggregate, AthleteId>, IAthleteRepository
{
    public AthleteRepository(ServiceDbContext dbContext)
        : base(dbContext)
    {
    }
}
