using Achievements.Application.Features.Achievements.Queries.GetAchievements;
using Achievements.Domain.Aggregates.Achievement;
using Mapster;

namespace Achievements.Application.MapperConfigurations;
public sealed class AchievementsConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Achievement, GetAchievementsQueryResponse>()
            .Map(dest => dest.CurrentLevel, src => src.AchievementLevels.Count);
    }
}
