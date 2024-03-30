using Athletes.Application.Features.Athletes.Queries.GetAuthorizedAthlete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Athletes.Api.Controllers;
[ApiController]
[Route("api/athlete")]
public class AthleteController : ControllerBase
{
    private readonly ILogger<AthleteController> _logger;
    private readonly ISender _mediatr;

    public AthleteController(ILogger<AthleteController> logger, ISender mediatr)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    [HttpGet]
    public async Task<IActionResult> GetAuthorizedAthlete()
    {
        var query = new GetAuthorizedAthleteQuery();
        var response = await _mediatr.Send(query);

        return Ok(response);
    }
}
