using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tiles.Application.Dtos.ActivityTiles;
using Tiles.Application.Features.ActivityTiles.Queries.GetAll;

namespace Tiles.Api.Controllers;
[ApiController]
[Route("api/tile")]
public sealed class ActivityTilesController : ControllerBase
{
    private readonly ILogger<ActivityTilesController> _logger;
    private readonly ISender _mediatr;

    public ActivityTilesController(ILogger<ActivityTilesController> logger, ISender mediatr)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityTilesResponse>>> GetAllActivityTiles()
    {
        var query = new GetAllActivityTilesQuery();
        var response = await _mediatr.Send(query);

        return Ok(response);
    }
}
