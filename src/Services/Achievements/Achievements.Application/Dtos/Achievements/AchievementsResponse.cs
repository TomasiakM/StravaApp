namespace Achievements.Application.Dtos.Achievements;
public record AchievementsResponse(
    int CurrentLevel,
    string AchievementType,
    List<object> Thresholds);
