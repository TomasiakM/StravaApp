using System.Text.Json.Serialization;

namespace Strava.Application.Dtos.Webhook;
public record SubscriptionValidationResponse(
    [property: JsonPropertyName("hub.challenge")] string HubChallenge);
