using Auth.Application.Interfaces;
using Auth.Domain.Aggregates.Token;
using Auth.Infrastructure.Interfaces.Services.StravaService;
using Auth.Infrastructure.Interfaces.Utils;

namespace Auth.Infrastructure.Utils;
internal sealed class TokenProvider : ITokenProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshStravaTokenService _refreshStravaTokenService;

    public TokenProvider(IUnitOfWork unitOfWork, IRefreshStravaTokenService refreshStravaTokenService)
    {
        _unitOfWork = unitOfWork;
        _refreshStravaTokenService = refreshStravaTokenService;
    }

    public async Task<TokenAggregate?> GetUserTokenAsync(long stravaUserId, CancellationToken cancellationToken)
    {
        var token = await _unitOfWork.Tokens.GetAsync(
            filter: e => e.StravaUserId == stravaUserId,
            cancellationToken: cancellationToken);

        if (token is null)
        {
            return null;
        }

        if (IsRefreshRequired(token))
        {
            var refreshResponse = await _refreshStravaTokenService.RefreshAsync(token.RefreshToken);

            token.Update(
                refreshResponse.RefreshToken,
                refreshResponse.AccessToken,
                refreshResponse.ExpiresAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return token;
    }

    private static bool IsRefreshRequired(TokenAggregate token)
    {
        var tokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(token.ExpiresAt);

        return DateTime.UtcNow.AddMinutes(30) > tokenExpiresAt;
    }
}
