using System.Text.Json.Serialization;

namespace Strava.Application.Dtos.Webhook;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public record StravaEventDataRequest
{
    [JsonPropertyName("object_type")]
    public string ObjectType { get; set; }
    [JsonPropertyName("object_id")]
    public long ObjectId { get; set; }
    [JsonPropertyName("aspect_type")]
    public string AspectType { get; set; }
    [JsonPropertyName("owner_id")]
    public long OwnerId { get; set; }
    [JsonPropertyName("subscription_id")]
    public int SubscriptionId { get; set; }
    [JsonPropertyName("event_time")]
    public long EventTime { get; set; }
    public UpdatesRequest Updates { get; set; }
}

public record UpdatesRequest
{
    public string? Title { get; set; }
    public string? Type { get; set; }
    public string? Private { get; set; }
    public string? Authorized { get; set; }
}