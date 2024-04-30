using Common.Domain.Interfaces;
using Tiles.Domain.Aggregates.ActivityTiles;

namespace Tiles.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    IActivityTilesRepository Tiles { get; }
}
