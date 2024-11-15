using Achievements.Domain.Aggregates.Achievement;
using Achievements.Domain.Aggregates.Achievement.Enums;
using Common.Domain.Interfaces;
using Common.Domain.Models;

namespace Achievements.Domain.Tests.Aggregates;
internal class TestAchievement : Achievement
{
    public const int MaxLevel = 8;
    private readonly List<int> s_thresholds = Enumerable.Range(1, MaxLevel).ToList();

    public TestAchievement(long stravaUserId, AchievementType achievementType) : base(stravaUserId, achievementType)
    {
    }

    public override IEnumerable<object> GetThresholds()
    {
        return s_thresholds.Select(e => (object)e);
    }

    public override void UpdateLevel(IEnumerable<Activity> activities, IDateProvider dateProvider)
    {
        throw new NotImplementedException();
    }

    public void SetLevelTest(int level, IDateProvider dateProvider)
    {
        SetLevel(level, dateProvider);
    }
}
