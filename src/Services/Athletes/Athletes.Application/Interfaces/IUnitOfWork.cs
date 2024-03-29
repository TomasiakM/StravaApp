using Athletes.Domain.Aggregates.Athletes;
using Common.Domain.Interfaces;

namespace Athletes.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    IAthleteRepository Athletes { get; }
}
