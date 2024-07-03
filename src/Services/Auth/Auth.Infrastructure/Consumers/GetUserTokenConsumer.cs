using Auth.Infrastructure.Interfaces.Utils;
using Common.MessageBroker.Contracts.Strava.GetUserToken;
using MassTransit;

namespace Auth.Infrastructure.Consumers;
internal sealed class GetUserTokenConsumer : IConsumer<GetUserTokenRequest>
{
    private readonly ITokenProvider _tokenProvider;

    public GetUserTokenConsumer(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public async Task Consume(ConsumeContext<GetUserTokenRequest> context)
    {
        var token = await _tokenProvider.GetUserTokenAsync(context.Message.StravaUserId);

        if (token is null)
        {
            await context.RespondAsync(new GetUserTokenNotFoundResponse());
            return;
        }

        await context.RespondAsync(new GetUserTokenResponse(token.AccessToken, token.RefreshToken));
    }
}
