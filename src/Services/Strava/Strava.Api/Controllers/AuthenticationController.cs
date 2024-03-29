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

        return Ok(loginResponse);
    }
}
