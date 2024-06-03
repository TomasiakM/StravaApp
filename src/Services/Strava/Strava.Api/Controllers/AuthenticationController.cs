using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strava.Application.Dtos.Auth;
using Strava.Application.Interfaces.Services.Auth;

namespace Strava.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILoginService loginService, IRefreshTokenService refreshTokenService, ILogger<AuthenticationController> logger)
    {
        _loginService = loginService;
        _refreshTokenService = refreshTokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request, CancellationToken cancellationToken)
    {
        var loginResponse = await _loginService.LoginAsync(request, cancellationToken);

        _logger.LogInformation("Athlete:{UserId} logged in.", loginResponse.Athlete.Id);

        return Ok(loginResponse);
    }

    [HttpPost("refresh")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Refreshing token.");

        var refreshResponse = await _refreshTokenService.RefreshTokenAsync(cancellationToken);

        return Ok(refreshResponse);
    }
}
