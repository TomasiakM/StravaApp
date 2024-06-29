using Auth.Domain.Aggregates.Token.ValueObjects;
using Common.Domain.DDD;

namespace Auth.Domain.Aggregates.Token;
public sealed class TokenAggregate : AggregateRoot<TokenId>
{
    public long StravaUserId { get; init; }
    public string RefreshToken { get; private set; }
    public string AccessToken { get; private set; }
    public int ExpiresAt { get; private set; }

    private TokenAggregate(long stravaUserId, string refreshToken, string accessToken, int expiresAt)
        : base(TokenId.Create())
    {
        StravaUserId = stravaUserId;
        RefreshToken = refreshToken;
        AccessToken = accessToken;
        ExpiresAt = expiresAt;
    }

    public static TokenAggregate Create(long stravaUserId, string refreshToken, string accessToken, int expiresAt)
        => new(stravaUserId, refreshToken, accessToken, expiresAt);

    public void Update(string refreshToken, string accessToken, int expiresAt)
    {
        RefreshToken = refreshToken;
        AccessToken = accessToken;
        ExpiresAt = expiresAt;
    }
}
