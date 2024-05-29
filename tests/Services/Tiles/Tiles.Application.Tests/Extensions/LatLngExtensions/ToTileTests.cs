using Common.Domain.Models;
using Tiles.Application.Extensions;

namespace Tiles.Application.Tests.Extensions.LatLngExtensions;
public class ToTileTests
{
    [Theory]
    [MemberData(nameof(TestCaseData))]
    public void Should_Create_Valid_Tile(LatLng latLon, int zoom, int x, int y)
    {
        var tile = latLon.ToTile(zoom);

        Assert.Equal(x, tile.X);
        Assert.Equal(y, tile.Y);
        Assert.Equal(zoom, tile.Z);
    }

    public static IEnumerable<object[]> TestCaseData()
    {
        yield return new object[] { LatLng.Create(23.023, 42.3321), 14, 10118, 7114 };
        yield return new object[] { LatLng.Create(33.063, 12.421), 14, 8757, 6596 };
        yield return new object[] { LatLng.Create(85.051128779806, -180), 14, 0, 0 };
        yield return new object[] { LatLng.Create(85.0493, -179.9789), 14, 0, 0 };
        yield return new object[] { LatLng.Create(85.0492, -179.9780), 14, 1, 1 };
    }
}
