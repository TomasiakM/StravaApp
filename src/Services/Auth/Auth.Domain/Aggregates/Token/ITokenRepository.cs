using Auth.Domain.Aggregates.Token.ValueObjects;
using Common.Domain.Interfaces;

namespace Auth.Domain.Aggregates.Token;
public interface ITokenRepository : IRepository<TokenAggregate, TokenId>
{
}
