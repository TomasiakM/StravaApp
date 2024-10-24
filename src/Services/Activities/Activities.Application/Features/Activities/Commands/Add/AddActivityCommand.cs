using Common.Domain.Enums;
using Common.Domain.Models;
using MediatR;

namespace Activities.Application.Features.Activities.Commands.Add;
public record AddActivityCommand(
    long Id,
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
    AthleteMetaCommand Athlete,
    MapSummaryCommand Map,
    StreamsCommand Streams) : IRequest<Unit>;

public record AthleteMetaCommand(
    long Id);

public record MapSummaryCommand(
    string Id,
    string? Polyline,
    string? SummaryPolyline);

public record StreamsCommand(
    List<int> Watts,
    List<int> Cadence,
    List<int> Heartrate,
    List<float> Altitude,
    List<float> Distance,
    List<LatLng> LatLngs);