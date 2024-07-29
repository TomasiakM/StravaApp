
using Common.Domain.Models;

namespace Achievements.Application.Interfaces.Services;
public interface IAllUserActivitiesService
{
    Task<IEnumerable<Activity>> GetAllAsync(long stravaUserId);
}
