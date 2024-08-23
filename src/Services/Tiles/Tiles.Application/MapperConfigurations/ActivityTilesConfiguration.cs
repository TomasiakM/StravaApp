using Mapster;
using Tiles.Application.Dtos.ActivityTiles;
using Tiles.Domain.Aggregates.ActivityTiles;

namespace Tiles.Application.MapperConfigurations;
public sealed class ActivityTilesConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ActivityTilesAggregate, ActivityTilesResponse>();
    }
}
