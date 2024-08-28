using Activities.Domain.Aggregates.Streams.ValueObjects;
using Common.Domain.Interfaces;

namespace Activities.Domain.Aggregates.Streams;
public interface IStreamRepository
    : IRepository<StreamAggregate, StreamId>
{
}
