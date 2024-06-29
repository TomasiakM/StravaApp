using Auth.Domain.Aggregates.Token;
using Common.Domain.Interfaces;

namespace Auth.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    public ITokenRepository Tokens { get; }
}
