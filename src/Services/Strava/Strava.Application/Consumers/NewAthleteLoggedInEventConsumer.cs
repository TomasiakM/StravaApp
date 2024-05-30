using Common.MessageBroker.Contracts.Activities;
using Common.MessageBroker.Contracts.Athletes;
using MassTransit;
using Microsoft.Extensions.Logging;
using Strava.Application.Interfaces.Services.StravaDataServices;

namespace Strava.Application.Consumers;
public class NewAthleteLoggedInEventConsumer
    : IConsumer<NewAthleteLoggedInEvent>
{
    private readonly ILogger<NewAthleteLoggedInEventConsumer> _logger;
    private readonly IAllUserActivitiesService _allUserActivitiesService;
    private readonly IBus _bus;
    public NewAthleteLoggedInEventConsumer(ILogger<NewAthleteLoggedInEventConsumer> logger, IAllUserActivitiesService allUserActivitiesService, IBus bus)
    {
        _logger = logger;
        _allUserActivitiesService = allUserActivitiesService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<NewAthleteLoggedInEvent> context)
    {
        var activities = await _allUserActivitiesService.GetAsync(context.Message.StravaUserId);

        foreach (var activity in activities)
        {
            _logger.LogInformation("[BUS] Sending activity event to fetch details for activity:{ActivityId}.", activity.Id);
            await _bus.Publish(new FetchAthleteActivityEvent(activity.Athlete.Id, activity.Id));
        }
    }
}
