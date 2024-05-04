using Common.Domain.DDD;

namespace Common.Domain.Models;
public sealed class LatLng : ValueObject
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    private LatLng(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static LatLng Create(double latitude, double longitude) => new(latitude, longitude);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }

    public LatLng() { }
}
