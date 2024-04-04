using Common.MessageBroker.Contracts.Activities;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces;

namespace Strava.Application.Consumers;
public sealed class FetchAthleteActivityEventConsumer
    : IConsumer<FetchAthleteActivityEvent>
{
    private readonly ILogger<FetchAthleteActivityEventConsumer> _logger;
    private readonly IStravaActivitiesService _stravaActivitiesService;
    private readonly IBus _bus;
    private readonly IMapper _mapper;

    public FetchAthleteActivityEventConsumer(ILogger<FetchAthleteActivityEventConsumer> logger, IStravaActivitiesService stravaActivitiesService, IBus bus, IMapper mapper)
    {
        _logger = logger;
        _stravaActivitiesService = stravaActivitiesService;
        _bus = bus;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<FetchAthleteActivityEvent> context)
    {
        var activity = await _stravaActivitiesService.GetActivityDetails(context.Message.StravaUserId, context.Message.StravaActivityId);

        if (activity is null)
        {
            _logger.LogWarning("Activity {ActivityId} is not found, user:{UserId}", context.Message.StravaActivityId, context.Message.StravaUserId);
            return;
        }

        _logger.LogInformation("[BUS] Sending event with detailed activity {ActivityId} \"{Name}\"", activity.Id, activity.Name);

        await _bus.Publish(_mapper.Map<ReceivedActivityDataEvent>(activity));
    }
}
