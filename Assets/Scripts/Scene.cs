using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public Transform chunksParent;

    [Header("Calculated")] public int initX;
    public int initY;
    public int endX;
    public int endY;

    public int Width => endX - initX;
    public int Height => endY - initY;

    public Dictionary<int, Map> map = new();

    public class Map
    {
        public Dictionary<Vector2Int, Chunk> chunks = new();

        public Tile GetTile(int x, int y)
        {
            int chunkX;
            int chunkY;
            int chunkTileX;
            int chunkTileY;
            if (x >= 0)
            {
                chunkX = x / World.ChunkSize;
                chunkTileX = x % World.ChunkSize;
            }
            else
            {
                chunkX = x / World.ChunkSize - 1;
                chunkTileX = 16 - x % World.ChunkSize;
            }

            if (y >= 0)
            {
                chunkY = y / World.ChunkSize;
                chunkTileY = y % World.ChunkSize;
            }
            else
            {
                chunkY = y / World.ChunkSize - 1;
                chunkTileY = 16 - y % World.ChunkSize;
            }

            var chunkKey = new Vector2Int(chunkX, chunkY);
            return chunks.TryGetValue(chunkKey, out var chunk) ? chunk.tiles[chunkTileX, chunkTileY] : null;
        }
    }
}