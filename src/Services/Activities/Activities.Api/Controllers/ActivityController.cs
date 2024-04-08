using Activities.Application.Features.Activities.Queries.GetAllActivities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Activities.Api.Controllers;
[ApiController]
[Route("api/activity")]
public class ActivityController : ControllerBase
{
    private readonly ILogger<ActivityController> _logger;
    private readonly ISender _mediatr;

    public ActivityController(ILogger<ActivityController> logger, ISender mediatr)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Sending query {Name}.", nameof(GetAllActivitiesQuery));

        var query = new GetAllActivitiesQuery();
        var response = await _mediatr.Send(query);

        _logger.LogInformation("Query {Name} was processed successfully.", nameof(GetAllActivitiesQuery));

        return Ok(response);
    }
}
