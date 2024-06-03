using Strava.Domain.Aggregates.Token;

namespace Strava.Infrastructure.Interfaces;
public interface IUserStravaTokenProvider
{
    Task<TokenAggregate?> GetTokenAsync(long stravaUserId, CancellationToken cancellationToken = default);
}
