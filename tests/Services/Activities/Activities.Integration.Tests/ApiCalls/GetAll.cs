using Activities.Application.Features.Activities.Queries.GetAllActivities;
using Activities.Application.Tests.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace Activities.Integration.Tests.ApiCalls;
public class GetAll : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public GetAll(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Should_Return401Status()
    {
        var response = await ServiceClient.GetAsync("api/activity");

        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }

    [Fact]
    public async Task Should_ReturnEmptyList()
    {
        AddToken(1);
        var response = await ServiceClient.GetAsync("api/activity");
        response.EnsureSuccessStatusCode();

        var responseResult = await response.Content.ReadFromJsonAsync<List<GetAllActivitiesQueryResponse>>();

        Assert.Empty(responseResult!);
    }

    [Fact]
    public async Task Should_ReturnList_WithUserActivities()
    {
        var userId = 4;
        AddToken(userId);

        var activity = Aggregates.CreateActivity(1, userId);
        var activity2 = Aggregates.CreateActivity(2, userId);
        await Insert(activity);
        await Insert(activity2);

        var response = await ServiceClient.GetAsync("api/activity");
        response.EnsureSuccessStatusCode();

        var responseResult = await response.Content.ReadFromJsonAsync<List<GetAllActivitiesQueryResponse>>();

        Assert.Equal(2, responseResult!.Count);
    }

    [Fact]
    public async Task Should_ReturnList_WithoutOtherUserActivities()
    {
        var userId = 4;
        AddToken(userId);

        var activity = Aggregates.CreateActivity(1, userId);
        var activity2 = Aggregates.CreateActivity(2, userId);
        await Insert(activity);
        await Insert(activity2);

        var otherUserId = 6;
        var activity3 = Aggregates.CreateActivity(3, otherUserId);
        await Insert(activity3);

        var response = await ServiceClient.GetAsync("api/activity");
        response.EnsureSuccessStatusCode();

        var responseResult = await response.Content.ReadFromJsonAsync<List<GetAllActivitiesQueryResponse>>();

        Assert.Equal(2, responseResult!.Count);
    }
}
