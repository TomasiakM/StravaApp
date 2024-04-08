using Activities.Application.Dtos.Activities;
using Activities.Domain.Aggregates.Activities;
using Common.Domain.Models;
using Mapster;

namespace Activities.Application.Mapper;
public sealed class ActivitiesConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ActivityAggregate, ActivityResponse>()
            .Map(dest => dest, src => src.Heartrate)
            .Map(dest => dest, src => src.Speed)
            .Map(dest => dest, src => src.Time)
            .Map(dest => dest, src => src.Watts)
            .Map(dest => dest, src => src.Map);

        config.NewConfig<LatLng, float[]>()
            .ConstructUsing(e => new float[]
                {
                    e.Latitude,
                    e.Longitude
                });
    }
}
