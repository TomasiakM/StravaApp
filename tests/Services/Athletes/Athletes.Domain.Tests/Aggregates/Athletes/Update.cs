using Athletes.Domain.Aggregates.Athletes;

namespace Athletes.Domain.Tests.Aggregates.Athletes;
public class Update
{
    [Fact]
    public void ShouldUpdateAthlete()
    {
        var stravaUserId = 3;
        var username = "username";
        var firstname = "firstname";
        var lastname = "last";
        var profile = "profile";
        var profileMedium = "profileMedium";
        var createdAt = new DateTime(2000, 1, 20, 10, 10, 10);

        var athlete = AthleteAggregate.Create(stravaUserId, username, firstname, lastname, profile, profileMedium, createdAt);

        var username2 = "username2";
        var firstname2 = "firstname2";
        var lastname2 = "last2";
        var profile2 = "profile2";
        var profileMedium2 = "profileMedium2";

        athlete.Update(username2, firstname2, lastname2, profile2, profileMedium2);

        Assert.Equal(stravaUserId, athlete.StravaUserId);
        Assert.Equal(username2, athlete.Username);
        Assert.Equal(firstname2, athlete.Firstname);
        Assert.Equal(lastname2, athlete.Lastname);
        Assert.Equal(profile2, athlete.Profile);
        Assert.Equal(profileMedium2, athlete.ProfileMedium);
        Assert.Equal(createdAt, athlete.CreatedAt);
    }
}
