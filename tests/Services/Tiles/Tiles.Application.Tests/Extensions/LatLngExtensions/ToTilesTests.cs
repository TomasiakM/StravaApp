using Common.Domain.Models;
using Tiles.Application.Extensions;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Application.Tests.Extensions.LatLngExtensions;
public class ToTilesTests
{
    [Fact]
    public void Should_Create_Convert_LatLng_To_Tile()
    {
        var latLngs = new List<LatLng>();
        var tiles = latLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);

        Assert.Equal(latLngs.Count, tiles.Count());
    }

    [Fact]
    public void Should_Create_Convert_LatLng_To_Tile2()
    {
        var latLngs = new List<LatLng>()
        {
            LatLng.Create(23.023, 42.3321), // 10118, 7114
            LatLng.Create(33.063, 12.421), // 8757, 6596
            LatLng.Create(85.0493, -179.9789), // 0, 0
            LatLng.Create(85.0492, -179.9780), // 1, 1
        };


        var tiles = latLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);

        Assert.Equal(4, tiles.Count());
    }

    [Fact]
    public void Should_Create_Convert_LatLng_To_Tile3()
    {
        var latLngs = new List<LatLng>()
        {
            LatLng.Create(23.023, 42.3321), // 10118, 7114
            LatLng.Create(33.063, 12.421), // 8757, 6596
            LatLng.Create(85.051128779806, -180), // 0, 0
            LatLng.Create(85.0493, -179.9789), // 0, 0
            LatLng.Create(85.0492, -179.9780), // 1, 1
        };


        var tiles = latLngs.ToUniqueTiles(Tile.DEFAULT_TILE_ZOOM);

        Assert.Equal(4, tiles.Count());
    }
}
