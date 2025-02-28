﻿using Activities.Application.Features.Activities.Queries.GetAllActivities;
using Activities.Domain.Aggregates.Activities;
using Common.Domain.Models;
using Mapster;

namespace Activities.Application.MapperConfigurations;
public sealed class ActivitiesConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ActivityAggregate, GetAllActivitiesQueryResponse>()
            .Map(dest => dest, src => src.Heartrate)
            .Map(dest => dest, src => src.Speed)
            .Map(dest => dest, src => src.Time)
            .Map(dest => dest, src => src.Watts)
            .Map(dest => dest, src => src.Map);

        config.NewConfig<LatLng, double[]>()
            .ConstructUsing(e => new double[]
                {
                    e.Latitude,
                    e.Longitude
                });

        config.NewConfig<ActivityAggregate, Activity>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Distance, src => src.Distance)
            .Map(dest => dest.StartDateLocal, src => src.Time.StartDateLocal);
    }
}
