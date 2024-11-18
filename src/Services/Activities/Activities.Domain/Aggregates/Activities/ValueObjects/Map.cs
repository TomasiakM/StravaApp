using Common.Domain.DDD;
using Common.Domain.Models;

namespace Activities.Domain.Aggregates.Activities.ValueObjects;
public sealed class Map : ValueObject
{
    public LatLng? StartLatlng { get; init; }
    public LatLng? EndLatlng { get; init; }

    public string? Polyline { get; init; }
    public string? SummaryPolyline { get; init; }

    private Map(LatLng? startLatlng, LatLng? endLatlng, string? polyline, string? summaryPolyline)
    {
        StartLatlng = startLatlng;
        EndLatlng = endLatlng;

        Polyline = polyline;
        SummaryPolyline = summaryPolyline;
    }

    public static Map Create(LatLng? startLatlng, LatLng? endLatlng, string? polyline, string? summaryPolyline)
        => new(startLatlng, endLatlng, polyline, summaryPolyline);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartLatlng;
        yield return EndLatlng;
        yield return Polyline;
        yield return SummaryPolyline;
    }

    private Map() { }
}
