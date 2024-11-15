namespace Achievements.Application.Features.Achievements.Queries.GetAchievements;

public record GetAchievementsQueryResponse(
    int CurrentLevel,
    string AchievementType,
    List<object> Thresholds);