using Microsoft.AspNetCore.Mvc;

namespace Activities.Api.Controllers;
[ApiController]
[Route("api/activity")]
public class ActivityController : ControllerBase
{
    private readonly ILogger<ActivityController> _logger;

    public ActivityController(ILogger<ActivityController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public Task<IActionResult> Get()
    {

        return Task.FromResult<IActionResult>(Ok());
    }
}
