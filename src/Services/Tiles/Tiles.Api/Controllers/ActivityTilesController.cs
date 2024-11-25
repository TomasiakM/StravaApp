using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetAllActivityTilesQueryResponse>>> GetAllActivityTiles()
    {
        var query = new GetAllActivityTilesQuery();
        var response = await _mediatr.Send(query);

        return Ok(response);
    }
}
