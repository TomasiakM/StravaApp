namespace Tiles.Application.Features.ActivityTiles.Queries.GetAll;
public record GetAllActivityTilesQueryResponse(
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