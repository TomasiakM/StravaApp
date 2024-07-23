using Achievements.Domain.Aggregates.Achievement;
using Common.Domain.Interfaces;

namespace Achievements.Application.Interfaces;
public interface IUnitOfWork : IBaseUnitOfWork
{
    IAchievementRepository Achievements { get; }
}
