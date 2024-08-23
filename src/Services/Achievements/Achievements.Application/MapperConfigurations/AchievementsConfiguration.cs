using Achievements.Application.Dtos.Achievements;
using Achievements.Domain.Aggregates.Achievement;
using Mapster;

namespace Achievements.Application.MapperConfigurations;
public sealed class AchievementsConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Achievement, AchievementsResponse>();
    }
}
