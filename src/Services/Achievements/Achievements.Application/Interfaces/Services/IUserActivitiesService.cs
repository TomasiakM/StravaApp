
using Common.Domain.Models;

namespace Achievements.Application.Interfaces.Services;
public interface IUserActivitiesService
{
    Task<IEnumerable<Activity>> GetAllAsync(long stravaUserId);
}
