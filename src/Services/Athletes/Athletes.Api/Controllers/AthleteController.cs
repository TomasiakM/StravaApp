using Athletes.Application.Features.Athletes.Queries.GetAuthorizedAthlete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> GetAuthorizedAthlete()
    {
        _logger.LogInformation("Sending query {Name}.", nameof(GetAuthorizedAthleteQuery));

        var query = new GetAuthorizedAthleteQuery();
        var response = await _mediatr.Send(query);

        _logger.LogInformation("Query {Name} was processed successfully.", nameof(GetAuthorizedAthlete));

        return Ok(response);
    }
}
