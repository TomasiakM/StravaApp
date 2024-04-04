using Strava.Contracts.Activity;

namespace Strava.Application.Interfaces;
public interface IStravaActivitiesService
{
    Task<ICollection<StravaActivitySummaryResponse>> GetAthleteActivities(long stravaUserId, CancellationToken cancellationToken = default);
    Task<StravaActivityDetailsResponse> GetActivityDetails(long stravaUserId, long stravaActivityId, CancellationToken cancellationToken = default);
}
