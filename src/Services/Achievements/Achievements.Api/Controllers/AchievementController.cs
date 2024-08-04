using Achievements.Application.Features.Achievements.Queries.GetAchievements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Achievements.Api.Controllers;
[ApiController]
[Route("api/achievement")]
public sealed class AchievementController : ControllerBase
{
    private readonly ISender _mediatr;

    public AchievementController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAchievements()
    {
        var query = new GetAchievementsQuery();
        var response = await _mediatr.Send(query);

        return Ok(response);
    }
}
