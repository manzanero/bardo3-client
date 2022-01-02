using System;
using Newtonsoft.Json;
using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    public Scene scene;

    public static SceneGenerator instance;
    
    private const float U = 1 / 16f;

    private void Awake()
    {
        instance = this;
    }

    public void CreateSceneFromDonjonJson(string textMap)
    {
        var file = Utils.GetResource<TextAsset>(textMap);
        var donjonMap = JsonConvert.DeserializeObject<Donjon.MapModel>(file.text);

        scene.map[0] = new Scene.Map();
        scene.initX = 0;
        scene.initY = 0;
        var lenX = scene.endX = 2 * donjonMap.cells[0].Count;
        var lenY = scene.endY = 2 * donjonMap.cells.Count;
        var lenChunkX = lenX % World.ChunkSize == 0 ? lenX / World.ChunkSize : lenX / World.ChunkSize + 1;
        var lenChunkY = lenY % World.ChunkSize == 0 ? lenY / World.ChunkSize : lenY / World.ChunkSize + 1;
        var rows = donjonMap.cells;
        rows.Reverse();

        for (var chunkY = 0; chunkY < lenChunkY; chunkY++)
        for (var chunkX = 0; chunkX < lenChunkX; chunkX++)
        {
            var chunk = Instantiate(World.instance.chunkPrefab,
                new Vector3(chunkX * World.ChunkSize, 0, chunkY * World.ChunkSize),
                Quaternion.identity, scene.chunksParent).GetComponent<Chunk>();

            chunk.name = $"Chunk__{chunkX}_{chunkY}";
            scene.map[0].chunks.Add(new Vector2Int(chunkX, chunkY), chunk);

            for (var y = 0; y < World.ChunkSize; y++)
            for (var x = 0; x < World.ChunkSize; x++)
            {
                var tile = chunk.tiles[x, y];
                var sceneX = Math.Clamp(x + chunkX * World.ChunkSize, 0, lenX - 1);
                var sceneY = Math.Clamp(y + chunkY * World.ChunkSize, 0, lenY - 1);
                tile.name = $"Tile__{sceneX}_{sceneY}";
                var cell = rows[sceneY / 2][sceneX / 2];

                if (cell is "0" or "16")
                {
                    tile.SetTileSet(TileManager.instance.atlas.DefaultWall, SetMethod.Random);
                    tile.height = (int) ((TileManager.instance.GetPerlinPosition(sceneX, sceneY) + 1) / U);
                }
                else
                {
                    tile.SetTileSet(TileManager.instance.atlas.DefaultFloor, SetMethod.Random);
                    tile.transparent = true;
                    tile.height = (int) (TileManager.instance.GetPerlinPosition(sceneX, sceneY) / U);
                }
            }
        }
    }
}