using Common.MessageBroker.Contracts.Activities;
using Common.MessageBroker.Saga.ProcessActivityData;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces.Services.StravaDataServices;

namespace Strava.Application.Consumers;
public sealed class FetchAthleteActivityEventConsumer
    : IConsumer<FetchAthleteActivityEvent>
{
    private readonly ILogger<FetchAthleteActivityEventConsumer> _logger;
    private readonly IUserActivityService _userActivityService;
    private readonly IActivityStreamsService _activityStreamsService;
    private readonly IBus _bus;
    private readonly IMapper _mapper;

    public FetchAthleteActivityEventConsumer(ILogger<FetchAthleteActivityEventConsumer> logger, IUserActivityService userActivityService, IActivityStreamsService activityStreamsService, IBus bus, IMapper mapper)
    {
        _logger = logger;
        _userActivityService = userActivityService;
        _activityStreamsService = activityStreamsService;
        _bus = bus;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<FetchAthleteActivityEvent> context)
    {
        var activity = await _userActivityService.GetAsync(context.Message.StravaUserId, context.Message.StravaActivityId);

        if (activity is null)
        {
            _logger.LogWarning("Activity:{ActivityId} is not found, user:{UserId}", context.Message.StravaActivityId, context.Message.StravaUserId);
            return;
        }

        var activityStreams = await _activityStreamsService.GetAsync(activity.Athlete.Id, activity.Id);

        _logger.LogInformation("[BUS] Sending saga message with detailed activity:{ActivityId} \"{Name}\"", activity.Id, activity.Name);
        await _bus.Publish(_mapper.Map<ProcessActivityDataMessage>((Guid.NewGuid(), activity, activityStreams)));
    }
}
