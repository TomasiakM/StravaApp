using Auth.Domain.Aggregates.Token;

namespace Auth.Domain.Tests.Aggregates.Token;
public class Update
{
    [Fact]
    public void ShouldUpdateToken()
    {
        var userId = 3;
        var accessToken = "accessT";
        var refreshToken = "refreshT";
        var exporesAt = 200;

        var token = TokenAggregate.Create(
            userId,
            refreshToken,
            accessToken,
            exporesAt);

        var accessToken2 = "accessT2";
        var refreshToken2 = "refreshT2";
        var exporesAt2 = 300;

        token.Update(refreshToken2, accessToken2, exporesAt2);

        Assert.Equal(userId, token.StravaUserId);
        Assert.Equal(accessToken2, token.AccessToken);
        Assert.Equal(refreshToken2, token.RefreshToken);
        Assert.Equal(exporesAt2, token.ExpiresAt);
    }
}
