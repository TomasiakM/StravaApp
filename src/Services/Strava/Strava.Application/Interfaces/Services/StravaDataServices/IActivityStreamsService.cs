using Strava.Application.Models;

namespace Strava.Application.Interfaces.Services.StravaDataServices;
public interface IActivityStreamsService
{
    Task<ActivityStreams> GetAsync(long stravaUserId, long stravaActivityId, CancellationToken cancellationToken = default);
}
