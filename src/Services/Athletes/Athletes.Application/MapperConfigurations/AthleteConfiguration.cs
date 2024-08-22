using Athletes.Application.Dtos.Athletes.Responses;
using Athletes.Domain.Aggregates.Athletes;
using Mapster;

namespace Athletes.Application.MapperConfigurations;
public sealed class AthleteConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AthleteAggregate, AthleteResponse>()
            .Map(dest => dest.Id, src => src.StravaUserId);
    }
}
