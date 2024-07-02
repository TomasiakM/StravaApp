using Auth.Domain.Aggregates.Token;

namespace Auth.Infrastructure.Interfaces.Utils;
internal interface ITokenProvider
{
    Task<TokenAggregate?> GetUserTokenAsync(long stravaUserId, CancellationToken cancellationToken = default);
}
