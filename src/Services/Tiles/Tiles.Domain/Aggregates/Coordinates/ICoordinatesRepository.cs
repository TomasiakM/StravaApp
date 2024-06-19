using Common.Domain.Interfaces;
using Tiles.Domain.Aggregates.Coordinates.ValueObjects;

namespace Tiles.Domain.Aggregates.Coordinates;
public interface ICoordinatesRepository
    : IRepository<CoordinatesAggregate, CoordinatesId>
{
}
