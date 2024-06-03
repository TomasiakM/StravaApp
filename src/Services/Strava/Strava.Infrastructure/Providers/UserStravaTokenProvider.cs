using Strava.Application.Interfaces;
using Strava.Domain.Aggregates.Token;
using Strava.Infrastructure.Interfaces;

namespace Strava.Infrastructure.Providers;
internal sealed class UserStravaTokenProvider : IUserStravaTokenProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshStravaUserTokenService _refreshStravaUserTokenService;

    public UserStravaTokenProvider(IUnitOfWork unitOfWork, IRefreshStravaUserTokenService refreshStravaUserTokenService)
    {
        _unitOfWork = unitOfWork;
        _refreshStravaUserTokenService = refreshStravaUserTokenService;
    }

    public async Task<TokenAggregate?> GetTokenAsync(long stravaUserId, CancellationToken cancellationToken = default)
    {
        var token = await _unitOfWork.Tokens.GetAsync(
            filter: e => e.StravaUserId == stravaUserId,
            cancellationToken: cancellationToken);

        if (token is null)
        {
            return null;
        }

        var tokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(token.ExpiresAt);
        if (DateTime.UtcNow.AddMinutes(30) > tokenExpiresAt)
        {
            var refreshResponse = await _refreshStravaUserTokenService.RefreshAsync(stravaUserId, cancellationToken);

            token.Update(
                refreshResponse.RefreshToken,
                refreshResponse.AccessToken,
                refreshResponse.ExpiresAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return token;
    }
}
