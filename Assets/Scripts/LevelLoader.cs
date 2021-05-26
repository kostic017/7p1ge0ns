using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Texture2D map;
    public Vector3 tileSize;
    public float tileSpacing;
    public LevelObject[] levelObjects;

    private Level level;

    private readonly Dictionary<Color, LevelObject> mappings = new Dictionary<Color, LevelObject>();

    void Awake()
    {
        level = new Level(map.width, map.height);

        var cx = map.width * (tileSize.x + tileSpacing) * 0.5f;
        Camera.main.transform.position = new Vector3(cx, Camera.main.transform.position.y, Camera.main.transform.position.z);
        
        foreach (var levelObject in levelObjects)
            mappings.Add(levelObject.mapColor, levelObject);

        for (var r = 0; r < map.height; ++r)
        {
            for (var c = 0; c < map.width; ++c)
            {
                var x = c * (tileSize.x + tileSpacing);
                var z = r * (tileSize.z + tileSpacing);
                
                var levelObject = mappings[map.GetPixel(c, r)];
                var position = new Vector3(x, Mathf.Abs(levelObject.height - tileSize.y) * 0.5f, z);
                var gameObject = Instantiate(levelObject.prefab, position, Quaternion.identity);
                
                level.Tiles[r, c] = levelObject.prefab.name;
                gameObject.transform.localScale = new Vector3(tileSize.x, levelObject.height, tileSize.z);

                if (levelObject.prefab.name == "Start")
                {
                    level.StartTiles.Add(new Vector2Int(c, r));
                    Camera.main.transform.LookAt(gameObject.transform);
                }
                else if (levelObject.prefab.name == "Finish")
                {
                    level.FinishTiles.Add(new Vector2Int(c, r));
                }
            }
        }
    }

}
