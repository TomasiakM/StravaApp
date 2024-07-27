namespace Achievements.Application.Dtos.Achievements;
public record AchievementsResponse(
    int CurrentLevel,
    string Type,
    List<object> Thresholds);
