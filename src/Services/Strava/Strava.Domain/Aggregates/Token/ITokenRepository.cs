using Common.Domain.Interfaces;
using Strava.Domain.Aggregates.Token.ValueObjects;

namespace Strava.Domain.Aggregates.Token;
public interface ITokenRepository : IRepository<TokenAggregate, TokenId>
{
}
