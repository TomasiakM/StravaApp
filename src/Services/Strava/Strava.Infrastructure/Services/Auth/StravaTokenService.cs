using Common.MessageBroker.Contracts.Strava.GetUserToken;
using MassTransit;
using Strava.Infrastructure.Interfaces.Auth;

namespace Strava.Infrastructure.Services.Auth;
public sealed class StravaTokenService : IStravaTokenService
{
    private readonly IRequestClient<GetUserTokenRequest> _client;

    public StravaTokenService(IRequestClient<GetUserTokenRequest> client)
    {
        _client = client;
    }

    public async Task<GetUserTokenResponse?> GetUserStravaTokens(long stravaUserId)
    {
        var response = await _client.GetResponse<GetUserTokenResponse, GetUserTokenNotFoundResponse>(stravaUserId);

        if (response.Is(out Response<GetUserTokenResponse> responseWithToken))
        {
            return responseWithToken.Message;
        }

        return null;
    }
}
