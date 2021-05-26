using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string[,] Tiles { get; private set; }
    public List<Vector2Int> StartTiles { get; } = new List<Vector2Int>();
    public List<Vector2Int> FinishTiles { get; } = new List<Vector2Int>();
    public List<GameObject> Enemies { get; } = new List<GameObject>();

    public Level(int w, int h)
    {
        Tiles = new string[h, w];
    }
}
