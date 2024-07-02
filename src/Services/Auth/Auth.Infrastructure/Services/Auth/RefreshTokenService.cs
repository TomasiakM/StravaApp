using Auth.Application.Dtos;
using Auth.Application.Interfaces.Services;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Interfaces.Utils;
using Common.Application.Interfaces;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Strava.Infrastructure.Services.Auth;
internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IUserIdProvider _userIdProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;
    private readonly ITokenProvider _tokenProvider;

    public RefreshTokenService(IUserIdProvider userIdProvider, IHttpContextAccessor httpContextAccessor, ITokenService tokenService, ITokenProvider tokenProvider)
    {
        _userIdProvider = userIdProvider;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
        _tokenProvider = tokenProvider;
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        var stravaUserId = _userIdProvider.GetUserId();
        var token = await _tokenProvider.GetUserTokenAsync(stravaUserId, cancellationToken);

        if (token is null)
        {
            await _httpContextAccessor.HttpContext!
                .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            throw new ForbiddenException();
        }

        var accessToken = _tokenService.GenerateToken(stravaUserId);

        return new RefreshTokenResponse(accessToken);
    }
}
