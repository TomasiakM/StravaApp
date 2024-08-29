using Activities.Domain.Aggregates.Streams;
using Activities.Domain.Aggregates.Streams.ValueObjects;
using Common.Infrastructure.Persistence;

namespace Activities.Infrastracture.Persistence.Repositories;
internal sealed class StreamRepository
    : GenericRepository<StreamAggregate, StreamId>, IStreamRepository
{
    public StreamRepository(ServiceDbContext dbContext)
        : base(dbContext)
    {
    }
}
