using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Texture2D map;
    public Vector3 tileSize;
    public float tileSpacing;
    public LevelObject[] levelObjects;
    public ProgrammableObject[] programmables;

    private Level level;

    private readonly Dictionary<Color, LevelObject> mappings = new Dictionary<Color, LevelObject>();

    void Awake()
    {
        level = new Level(map.width, map.height);

        foreach (var levelObject in levelObjects)
            mappings.Add(levelObject.mapColor, levelObject);

        for (var r = 0; r < map.height; ++r)
        {
            for (var c = 0; c < map.width; ++c)
            {
                var x = c * (tileSize.x + tileSpacing);
                var z = r * (tileSize.z + tileSpacing);
                
                var levelObject = mappings[map.GetPixel(c, r)];
                var position = new Vector3(x, levelObject.height * 0.5f, z);
                var gameObject = Instantiate(levelObject.prefab, position, Quaternion.identity);
                
                level.Tiles[r, c] = levelObject;
                gameObject.transform.localScale = new Vector3(tileSize.x, levelObject.height, tileSize.z);

                if (levelObject.prefab.name == "Spawner")
                {
                    level.Spawners.Add(new Vector2Int(c, r));
                }
                else if (levelObject.prefab.name == "Finish")
                {
                    level.FinishTiles.Add(new Vector2Int(c, r));
                }
            }
        }

        foreach (var programmable in programmables)
        {
            var x = programmable.position.x * (tileSize.x + tileSpacing);
            var z = programmable.position.y * (tileSize.z + tileSpacing);
            var y = level.Tiles[programmable.position.y, programmable.position.x].height;
            Instantiate(programmable.prefab, new Vector3(x, y, z), Quaternion.identity);
        }
    }

}
