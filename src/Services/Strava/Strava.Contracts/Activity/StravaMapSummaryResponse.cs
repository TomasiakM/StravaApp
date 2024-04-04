namespace Strava.Contracts.Activity;
public record StravaMapSummaryResponse(
    string Id,
    string? Polyline,
    string? SummaryPolyline);
