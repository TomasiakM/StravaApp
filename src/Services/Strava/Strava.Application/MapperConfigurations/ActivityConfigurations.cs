using Common.MessageBroker.Contracts.Activities;
using Mapster;
using Strava.Application.Models;
using Strava.Contracts.Activity;

namespace Strava.Application.MapperConfigurations;
internal class ActivityConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(StravaActivityDetailsResponse activityDetails, ActivityStreams streams), ReceivedActivityDataEvent>()
            .Map(dest => dest, src => src.activityDetails)
            .Map(dest => dest.Streams, src => src.streams);
    }
}
