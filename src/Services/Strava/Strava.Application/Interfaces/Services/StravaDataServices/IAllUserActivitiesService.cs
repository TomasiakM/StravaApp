using Strava.Contracts.Activity;

namespace Strava.Application.Interfaces.Services.StravaDataServices;
public interface IAllUserActivitiesService
{
    Task<IEnumerable<StravaActivitySummaryResponse>> GetAsync(long stravaUserId, CancellationToken cancellationToken = default);
}
