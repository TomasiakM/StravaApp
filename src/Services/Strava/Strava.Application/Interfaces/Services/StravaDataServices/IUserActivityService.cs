using Strava.Contracts.Activity;

namespace Strava.Application.Interfaces.Services.StravaDataServices;
public interface IUserActivityService
{
    Task<StravaActivityDetailsResponse> GetAsync(long stravaUserId, long stravaActivityId, CancellationToken cancellationToken = default);
}
