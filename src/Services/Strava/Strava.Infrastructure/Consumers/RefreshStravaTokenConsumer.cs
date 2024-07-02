using Common.MessageBroker.Contracts.Authorization.RefreshStravaToken;
using MassTransit;
using Strava.Infrastructure.Interfaces;

namespace Strava.Infrastructure.Consumers;
internal sealed class RefreshStravaTokenConsumer
    : IConsumer<RefreshStravaTokenRequest>
{
    private readonly IRefreshStravaUserTokenService _refreshStravaUserTokenService;

    public RefreshStravaTokenConsumer(IRefreshStravaUserTokenService refreshStravaUserTokenService)
    {
        _refreshStravaUserTokenService = refreshStravaUserTokenService;
    }

    public async Task Consume(ConsumeContext<RefreshStravaTokenRequest> context)
    {
        var refreshData = await _refreshStravaUserTokenService.RefreshAsync(context.Message.RefreshToken);

        var response = new RefreshStravaTokenResponse(
            refreshData.AccessToken,
            refreshData.RefreshToken,
            refreshData.ExpiresAt);

        await context.RespondAsync(response);
    }
}
