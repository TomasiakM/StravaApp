using Common.Domain.DDD;

namespace Common.Domain.Models;
public sealed class LatLng : ValueObject
{
    public float Latitude { get; init; }
    public float Longitude { get; init; }

    private LatLng(float latitude, float longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static LatLng Create(float latitude, float longitude) => new(latitude, longitude);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
