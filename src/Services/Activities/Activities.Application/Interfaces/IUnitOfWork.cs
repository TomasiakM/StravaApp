using Activities.Domain.Aggregates.Activities;
using Activities.Domain.Aggregates.Streams;
using Common.Domain.Interfaces;

namespace Activities.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    IActivityRepository Activities { get; }
    IStreamRepository Streams { get; }
}
