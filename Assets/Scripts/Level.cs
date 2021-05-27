using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public LevelObject[,] Tiles { get; private set; }
    public List<Vector2Int> Spawners { get; } = new List<Vector2Int>();
    public List<Vector2Int> FinishTiles { get; } = new List<Vector2Int>();

    public Level(int w, int h)
    {
        Tiles = new LevelObject[h, w];
    }
}
