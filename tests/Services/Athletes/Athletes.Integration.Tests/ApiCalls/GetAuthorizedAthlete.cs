using Athletes.Application.Dtos.Athletes.Responses;
using Athletes.Domain.Aggregates.Athletes;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace Athletes.Integration.Tests.ApiCalls;
public class GetAuthorizedAthlete : BaseTest, IClassFixture<IntegrationTestWebAppFactory>
{
    public GetAuthorizedAthlete(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Should_Return401Response_WhenTokenIsNotProvided()
    {
        var response = await ServiceClient.GetAsync("/api/athlete");

        Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
    }

    [Fact]
    public async Task Should_Return404Response_WhenAthleteIsNotFound()
    {
        var athleteId = 5;
        AddToken(athleteId);

        var response = await ServiceClient.GetAsync("/api/athlete");

        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }

    [Fact]
    public async Task Should_ReturnAthleteDetails()
    {
        var athleteId = 5;
        var athlete = AthleteAggregate.Create(athleteId, "TestUser", "TestName", "TestLast", "TestProfile", "TestProfileMedium", new(2022, 1, 1));
        await Insert(athlete);
        AddToken(athleteId);

        var response = await ServiceClient.GetAsync("/api/athlete");

        var responseResult = await response.Content.ReadFromJsonAsync<AthleteResponse>();
        Assert.NotNull(responseResult);
        Assert.Equal(athlete.StravaUserId, responseResult.Id);
        Assert.Equal(athlete.Username, responseResult.Username);
        Assert.Equal(athlete.Firstname, responseResult.Firstname);
        Assert.Equal(athlete.Lastname, responseResult.Lastname);
        Assert.Equal(athlete.Profile, responseResult.Profile);
        Assert.Equal(athlete.ProfileMedium, responseResult.ProfileMedium);
    }
}
