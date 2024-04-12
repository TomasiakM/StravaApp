using Microsoft.AspNetCore.Mvc;

namespace Strava.Application.Dtos.Webhook;
public record SubscriptionValidationParams(
    [ModelBinder(Name = "hub.mode")] string HubMode,
    [ModelBinder(Name = "hub.challenge")] string HubChallenge,
    [ModelBinder(Name = "hub.verify_token")] string HubVerifyToken
);
