using Common.Domain.Interfaces;
using Strava.Domain.Aggregates.Token;

namespace Strava.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    ITokenRepository Tokens { get; }
}
