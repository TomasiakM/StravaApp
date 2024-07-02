using Auth.Infrastructure.Interfaces.Services.StravaService;
using Common.MessageBroker.Contracts.Authorization.AuthorizeUserCode;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Services.StravaService;
internal sealed class AuthorizeCodeService : IAuthorizeCodeService
{
    private readonly IRequestClient<AuthorizeCodeRequest> _client;
    private readonly ILogger<AuthorizeCodeService> _logger;

    public AuthorizeCodeService(IRequestClient<AuthorizeCodeRequest> authorizeUserCodeClient, ILogger<AuthorizeCodeService> logger)
    {
        _client = authorizeUserCodeClient;
        _logger = logger;
    }

    public async Task<AuthorizeCodeResponse> AuthorizeAsync(string code)
    {
        _logger.LogInformation("Sending authorization code to strava service");

        var result = await _client
            .GetResponse<AuthorizeCodeResponse>(new AuthorizeCodeRequest(code));

        _logger.LogInformation("Successfully authorized code, user:{Username} logged in", result.Message.Athlete.Username);

        return result.Message;
    }
}
