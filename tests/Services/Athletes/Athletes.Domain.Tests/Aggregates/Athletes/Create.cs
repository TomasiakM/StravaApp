using Athletes.Domain.Aggregates.Athletes;

namespace Athletes.Domain.Tests.Aggregates.Athletes;
public class Create
{
    [Fact]
    public void ShouldCreateAthleteAggregate()
    {
        var stravaUserId = 3;
        var username = "username";
        var firstname = "firstname";
        var lastname = "last";
        var profile = "profile";
        var profileMedium = "profileMedium";
        var createdAt = new DateTime(2000, 1, 20, 10, 10, 10);

        var athlete = AthleteAggregate.Create(stravaUserId, username, firstname, lastname, profile, profileMedium, createdAt);

        Assert.Equal(stravaUserId, athlete.StravaUserId);
        Assert.Equal(username, athlete.Username);
        Assert.Equal(firstname, athlete.Firstname);
        Assert.Equal(lastname, athlete.Lastname);
        Assert.Equal(profile, athlete.Profile);
        Assert.Equal(profileMedium, athlete.ProfileMedium);
        Assert.Equal(createdAt, athlete.CreatedAt);
    }
}
