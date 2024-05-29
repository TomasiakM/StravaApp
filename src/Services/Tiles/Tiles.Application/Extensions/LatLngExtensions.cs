using Common.Domain.Models;
using Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;

namespace Tiles.Application.Extensions;
public static class LatLngExtensions
{
    public static IEnumerable<Tile> ToUniqueTiles(this IEnumerable<LatLng> latLngs, int zoom)
    {
        var tiles = new HashSet<Tile>();

        foreach (var latLng in latLngs)
        {
            tiles.Add(latLng.ToTile(zoom));
        }

        return tiles;
    }

    public static Tile ToTile(this LatLng latLng, int zoom)
    {
        int xyTilesCount = (int)Math.Pow(2, zoom);
        int x = (int)Math.Truncate(Math.Floor((latLng.Longitude + 180.0) / 360.0 * xyTilesCount));
        int y = (int)Math.Truncate(
          Math.Floor(
            (1.0 -
              Math.Log(
                Math.Tan(latLng.Latitude * Math.PI / 180.0) +
                  1.0 / Math.Cos(latLng.Latitude * Math.PI / 180.0)
              ) / Math.PI) / 2.0 * xyTilesCount
          )
        );


        return Tile.Create(x, y, zoom);
    }
}
