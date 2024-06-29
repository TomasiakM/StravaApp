using Auth.Domain.Aggregates.Token;
using Auth.Domain.Aggregates.Token.ValueObjects;
using Common.Infrastructure.Persistence;

namespace Auth.Infrastructure.Persistence.Repositories;
internal sealed class TokenRepository
    : GenericRepository<TokenAggregate, TokenId>, ITokenRepository
{
    public TokenRepository(ServiceDbContext dbContext)
        : base(dbContext) { }
}
