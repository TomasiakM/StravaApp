using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Strava.Application.Dtos.Webhook;
using Strava.Application.Features.StravaHook.HandleEvent;
using Strava.Infrastructure.Settings;

namespace Strava.Api.Controllers;

[ApiController]
[Route("api/webhook")]
public class StravaWebhookController : ControllerBase
{
    private readonly ILogger<StravaWebhookController> _logger;
    private readonly StravaSettings _stravaSettings;
    private readonly ISender _mediatr;
    private readonly IMapper _mapper;

    public StravaWebhookController(ILogger<StravaWebhookController> logger, IOptions<StravaSettings> stravaSettings, ISender mediatr, IMapper mapper)
    {
        _logger = logger;
        _stravaSettings = stravaSettings.Value;
        _mediatr = mediatr;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> GetEventData(StravaEventDataRequest request)
    {
        _logger.LogInformation("Received strava event data: {Request}.", request);

        var command = _mapper.Map<HandleEventCommand>(request);
        await _mediatr.Send(command);

        return Ok();
    }

    [HttpGet]
    public ActionResult<SubscriptionValidationResponse> ConfirmSubscription([FromQuery] SubscriptionValidationParams query)
    {
        if (query.HubMode == "subscribe" && query.HubVerifyToken == _stravaSettings.HubVerifyToken)
        {
            var response = new SubscriptionValidationResponse(query.HubChallenge);

            _logger.LogInformation("[Strava hook] Strava hook subscription confirmed.");

            return Ok(response);
        }

        _logger.LogWarning("[Strava hook] Strava hook subscription failed. {Request}.", query);

        return Unauthorized();
    }
}
