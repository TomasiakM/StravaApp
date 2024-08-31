using Common.MessageBroker.Saga.ProcessActivityData;
using Mapster;
using Strava.Application.Models;
using Strava.Contracts.Activity;

namespace Strava.Application.MapperConfigurations;
internal class ActivityConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid guid, StravaActivityDetailsResponse activityDetails, ActivityStreams streams), ProcessActivityDataMessage>()
            .Map(dest => dest.CorrelationId, src => src.guid)
            .Map(dest => dest, src => src.activityDetails)
            .Map(dest => dest.Streams, src => src.streams);
    }
}
