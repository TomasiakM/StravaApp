﻿using Common.Domain.Enums;

namespace Activities.Application.Features.Activities.Queries.GetAllActivities;
public record GetAllActivitiesQueryResponse(
    long StravaId,
    string Name,
    float Distance,
    int MovingTime,
    int ElapsedTime,
    float TotalElevationGain,
    SportType SportType,
    DateTime StartDate,
    DateTime StartDateLocal,
    double[] StartLatlng,
    double[] EndLatlng,
    bool Private,
    float AverageSpeed,
    float MaxSpeed,
    float AverageCadence,
    float AverageWatts,
    int MaxWatts,
    bool DeviceWatts,
    float Kilojoules,
    float Calories,
    string DeviceName,
    bool HasHeartrate,
    float AverageHeartrate,
    float MaxHeartrate,
    string Polyline,
    string SummaryPolyline);
