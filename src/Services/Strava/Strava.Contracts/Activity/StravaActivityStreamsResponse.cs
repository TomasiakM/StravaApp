using System.Text.Json.Serialization;

namespace Strava.Contracts.Activity;
public record StravaActivityStreamResponse(
    StreamType Type,
    List<dynamic> Data,
    int OriginalSize,
    Resolution Resolution,
    string SeriesType);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StreamType
{
    Distance,
    Latlng,
    Altitude,
    Heartrate,
    Cadence,
    Watts,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Resolution
{
    Low,
    Medium,
    High
}
