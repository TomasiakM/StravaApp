using Common.Infrastructure.Persistence;
using Strava.Domain.Aggregates.Token;
using Strava.Domain.Aggregates.Token.ValueObjects;

namespace Strava.Infrastructure.Persistence.Repositories;
internal sealed class TokenRepository
    : GenericRepository<TokenAggregate, TokenId>, ITokenRepository
{
    public TokenRepository(ServiceDbContext dbContext)
        : base(dbContext) { }
}
