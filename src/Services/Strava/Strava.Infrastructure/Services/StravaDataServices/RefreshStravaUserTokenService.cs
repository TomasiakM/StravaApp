using Common.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces;
using Strava.Contracts.Authorization;
using Strava.Infrastructure.HttpClients;
using Strava.Infrastructure.Interfaces;

namespace Strava.Infrastructure.Services.StravaDataServices;
internal sealed class RefreshStravaUserTokenService : IRefreshStravaUserTokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly StravaAuthenticationHttpClientService _stravaAuthenticationHttpClientService;
    private readonly ILogger<RefreshStravaUserTokenService> _logger;

    public RefreshStravaUserTokenService(IUnitOfWork unitOfWork, StravaAuthenticationHttpClientService stravaAuthenticationHttpClientService, ILogger<RefreshStravaUserTokenService> logger)
    {
        _unitOfWork = unitOfWork;
        _stravaAuthenticationHttpClientService = stravaAuthenticationHttpClientService;
        _logger = logger;
    }

    public async Task<StravaRefreshTokenResponse> RefreshAsync(long stravaUserId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Refreshing user:{UserId} token.", stravaUserId);

        var userStravaToken = await _unitOfWork.Tokens
            .GetAsync(e => e.StravaUserId == stravaUserId, cancellationToken: cancellationToken);

        if (userStravaToken is null)
        {
            _logger.LogWarning("Token is not found for user:{UserId}", stravaUserId);
            throw new UnauthorizedException();
        }

        var formData = new Dictionary<string, string>()
        {
            { "refresh_token", userStravaToken.RefreshToken },
            { "grand_type", "refresh_token"}
        };

        var response = await _stravaAuthenticationHttpClientService
            .PostRequest<StravaRefreshTokenResponse>(formData, cancellationToken);

        _logger.LogInformation("Sucessfully refreshed user:{UserId} token.", stravaUserId);

        return response;
    }
}
