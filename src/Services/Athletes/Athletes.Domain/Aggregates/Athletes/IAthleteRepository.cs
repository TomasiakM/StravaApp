using Athletes.Domain.Aggregates.Athletes.ValueObjects;
using Common.Domain.Interfaces;

namespace Athletes.Domain.Aggregates.Athletes;
public interface IAthleteRepository
    : IRepository<AthleteAggregate, AthleteId>
{
}
