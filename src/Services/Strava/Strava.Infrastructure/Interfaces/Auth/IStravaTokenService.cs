using Common.MessageBroker.Contracts.Strava.GetUserToken;

namespace Strava.Infrastructure.Interfaces.Auth;
internal interface IStravaTokenService
{
    Task<GetUserTokenResponse?> GetUserStravaTokens(long stravaUserId);
}
