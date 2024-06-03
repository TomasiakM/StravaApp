using Strava.Contracts.Authorization;

namespace Strava.Infrastructure.Interfaces;
public interface IConfirmStravaAuthenticationCodeService
{
    Task<StravaAuthorizationResponse> AuthorizeAsync(string code, CancellationToken cancellationToken = default);
}
