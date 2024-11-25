using Mapster;
using Tiles.Application.Features.ActivityTiles.Queries.GetAll;
using Tiles.Domain.Aggregates.ActivityTiles;

namespace Tiles.Application.MapperConfigurations;
public sealed class ActivityTilesConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ActivityTilesAggregate, GetAllActivityTilesQueryResponse>();
    }
}
