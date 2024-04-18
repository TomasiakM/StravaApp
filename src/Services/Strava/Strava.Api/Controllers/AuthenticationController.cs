using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Strava.Application.Dtos.Auth;
using Strava.Application.Interfaces;

namespace Strava.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IStravaAuthenticationService _stravaAuthenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IStravaAuthenticationService stravaAuthenticationService, ILogger<AuthenticationController> logger)
    {
        _stravaAuthenticationService = stravaAuthenticationService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request, CancellationToken cancellationToken)
    {
        var loginResponse = await _stravaAuthenticationService.LoginAsync(request, cancellationToken);

        _logger.LogInformation("Athlete:{UserId} logged in.", loginResponse.Athlete.Id);

        return Ok(loginResponse);
    }

    [HttpPost("refresh")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken()
    {
        _logger.LogInformation("Refreshing token.");

        var refreshResponse = await _stravaAuthenticationService.RefreshToken();

        return Ok(refreshResponse);
    }
}
