using Common.Application.Interfaces;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Strava.Application.Dtos.Auth;
using Strava.Application.Interfaces.Services.Auth;
using Strava.Infrastructure.Interfaces;

namespace Strava.Infrastructure.Services.Auth;
internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IUserIdProvider _userIdProvider;
    private readonly IUserStravaTokenProvider _userStravaTokneProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;

    public RefreshTokenService(IUserIdProvider userIdProvider, IUserStravaTokenProvider userStravaTokneProvider, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
    {
        _userIdProvider = userIdProvider;
        _userStravaTokneProvider = userStravaTokneProvider;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var stravaUserId = _userIdProvider.GetUserId();
        var token = await _userStravaTokneProvider.GetTokenAsync(stravaUserId, cancellationToken);

        if (token is null)
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            throw new ForbiddenException();
        }

        var accessToken = _tokenService.GenerateToken(stravaUserId);

        return new RefreshTokenResponse(accessToken);
    }
}
