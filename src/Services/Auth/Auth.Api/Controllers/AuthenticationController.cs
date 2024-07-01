using Auth.Application.Dtos;
using Auth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILoginService loginService, ILogger<AuthenticationController> logger)
    {
        _loginService = loginService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var loginResponse = await _loginService.LoginAsync(request, cancellationToken);

        _logger.LogInformation("Athlete:{UserId} logged in.", loginResponse.Athlete.Id);

        return Ok(loginResponse);
    }
}
