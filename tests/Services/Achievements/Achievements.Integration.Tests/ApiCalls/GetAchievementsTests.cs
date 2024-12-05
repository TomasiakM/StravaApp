using Achievements.Application.Features.Achievements.Queries.GetAchievements;
using Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements;
using Achievements.Domain.Aggregates.Achievement.Enums;
using Achievements.Domain.Aggregates.Achievement.Factories;
using Common.Application.Providers;
using Common.Domain.Models;
using Mapster.Utils;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace Achievements.Integration.Tests.ApiCalls;

public class GetAchievementsTests : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public GetAchievementsTests(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task ShouldReturn401_WhenTokenIsNotProvided()
    {
        var response = await ServiceClient.GetAsync("/api/achievement");

        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturn200_WithAchievementsList_WhenTokenIsProvided()
    {
        var userId = 3;
        AddToken(userId);
        var allAchievements = new AchievementFactory().CreateAll(userId);


        var response = await ServiceClient.GetAsync("/api/achievement");
        response.EnsureSuccessStatusCode();

        var responseResult = await response.Content.ReadFromJsonAsync<List<GetAchievementsQueryResponse>>();


        Assert.NotNull(responseResult);
        Assert.Equal(allAchievements.Count, responseResult.Count);
        Assert.True(responseResult.All(e => e.CurrentLevel == 0));
    }

    [Fact]
    public async Task ShouldReturn200_WithAchievementsListContainingCumulativeAchievementHavingLevel3()
    {
        var userId = 54;
        AddToken(userId);
        var allAchievements = new AchievementFactory().CreateAll(userId);

        var achievement = new CumulativeDistanceAchievement(userId);
        achievement.UpdateLevel(GetActivities(), new DateProvider());
        await Insert(achievement);


        var response = await ServiceClient.GetAsync("/api/achievement");
        response.EnsureSuccessStatusCode();

        var responseResult = await response.Content.ReadFromJsonAsync<List<GetAchievementsQueryResponse>>();


        var cumulative = responseResult!.First(e => e.AchievementType == Enum<AchievementType>.ToString(AchievementType.CumulativeDistance));
        Assert.Equal(3, cumulative.CurrentLevel);
    }

    private static IEnumerable<Activity> GetActivities()
    {
        yield return new(Guid.NewGuid(), 100000, new(2022, 1, 1));
        yield return new(Guid.NewGuid(), 200000, new(2022, 1, 2));
        yield return new(Guid.NewGuid(), 300000, new(2022, 1, 3));
    }
}
