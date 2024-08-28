using Common.Domain.Enums;
using Common.Domain.Models;

namespace Common.MessageBroker.Contracts.Activities;
public record ReceivedActivityDataEvent(
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
    AthleteMetaEvent Athlete,
    MapSummaryEvent Map,
    StreamsEvent Streams);

public record AthleteMetaEvent(
    long Id);

public record MapSummaryEvent(
    string Id,
    string? Polyline,
    string? SummaryPolyline);

public record StreamsEvent(
    List<int> Watts,
    List<int> Cadence,
    List<int> Heartrate,
    List<float> Altitude,
    List<float> Distance,
    List<LatLng> LatLngs);
