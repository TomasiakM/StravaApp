using Common.Domain.Enums;
using Common.Domain.Models;

namespace Common.MessageBroker.Saga.ProcessActivityData.Messages;
public record ProcessActivityMessage(
    Guid CorrelationId,
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
    AthleteMetaMessage Athlete,
    MapSummaryMessage Map,
    StreamsMessage Streams);

public record AthleteMetaMessage(
    long Id);

public record MapSummaryMessage(
    string Id,
    string? Polyline,
    string? SummaryPolyline);

public record StreamsMessage(
    List<int> Watts,
    List<int> Cadence,
    List<int> Heartrate,
    List<float> Altitude,
    List<float> Distance,
    List<LatLng> LatLngs);