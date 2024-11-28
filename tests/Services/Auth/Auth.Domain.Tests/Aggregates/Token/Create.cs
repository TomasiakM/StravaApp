using Auth.Domain.Aggregates.Token;

namespace Auth.Domain.Tests.Aggregates.Token;
public class Create
{
    [Fact]
    public void ShouldCreateTokenAggregate()
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

        Assert.Equal(userId, token.StravaUserId);
        Assert.Equal(accessToken, token.AccessToken);
        Assert.Equal(refreshToken, token.RefreshToken);
        Assert.Equal(exporesAt, token.ExpiresAt);
    }
}
