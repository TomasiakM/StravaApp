using Common.Domain.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;
using Tiles.Domain.Aggregates.Coordinates;

namespace Tiles.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    IActivityTilesRepository Tiles { get; }
    ICoordinatesRepository Coordinates { get; }
}
