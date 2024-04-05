using Activities.Domain.Aggregates.Activities;
using Common.Domain.Interfaces;

namespace Activities.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    IActivityRepository Activities { get; }
}
