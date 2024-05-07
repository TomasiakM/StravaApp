﻿namespace Tiles.Domain.Aggregates.ActivityTiles.ValueObjects;
public sealed class NewTile : Tile
{
    private NewTile(int x, int y, int z)
        : base(x, y, z) { }

    public new static NewTile Create(int x, int y, int z) => new(x, y, z);
}
