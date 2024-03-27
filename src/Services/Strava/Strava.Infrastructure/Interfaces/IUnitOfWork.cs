using Common.Domain.Interfaces;
using Strava.Domain.Aggregates.Token;

namespace Strava.Infrastructure.Interfaces;
internal interface IUnitOfWork : IBaseUnitOfWork
{
    ITokenRepository TokenRepository { get; }
}
