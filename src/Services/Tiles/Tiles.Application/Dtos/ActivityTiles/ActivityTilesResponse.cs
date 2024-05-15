namespace Tiles.Application.Dtos.ActivityTiles;
public record ActivityTilesResponse(
    long StravaUserId,
    long StravaActivityId,
    int NewSquare,
    ICollection<TileResponse> Tiles,
    ICollection<TileResponse> NewTiles,
    ICollection<TileResponse> NewClusterTiles);

public record TileResponse(
    int X,
    int Y,
    int Z);
