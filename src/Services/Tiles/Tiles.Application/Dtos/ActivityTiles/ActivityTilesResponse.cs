namespace Tiles.Application.Dtos.ActivityTiles;
public record ActivityTilesResponse(
    long StravaUserId,
    long StravaActivityId,
    ICollection<TileResponse> Tiles);

public record TileResponse(
    int X,
    int Y,
    int Z);
