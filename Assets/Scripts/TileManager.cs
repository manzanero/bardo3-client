using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public TileAtlas atlas;

    public Texture2D atlasTexture;
    public readonly Dictionary<TileSet, int> atlasSetIndexes = new();
    public readonly Dictionary<TileBlueprint, int> atlasTextureIndexes = new();
    public Texture2D perlinTexture;

    public static TileManager instance;

    private int perlinWidth;
    private int perlinHeight;
    private int perlinOffsetX;
    private int perlinOffsetY;
    private int perlinScale;

    private void Awake()
    {
        instance = this;
        perlinWidth = 256;
        perlinHeight = 256;
        perlinOffsetX = Range(0, 256);
        perlinOffsetY = Range(0, 256);
        perlinScale = Range(16, 32);
        RefreshPerlinTexture();
    }

    public void RefreshTileAtlasTexture()
    {
        var totalBlueprints = atlas.sets.Sum(set => set.blueprints.Count);
        atlasTexture = new Texture2D(16 * totalBlueprints, 32);

        var index = 0;
        foreach (var set in atlas.sets)
        {
            atlasSetIndexes.Add(set, index);

            foreach (var blueprint in set.blueprints)
            {
                atlasTextureIndexes.Add(blueprint, index);

                // top
                for (var y = 0; y < 16; y++)
                for (var x = 0; x < 16; x++)
                    atlasTexture.SetPixel(x + index * 16, y + 16, blueprint.top.GetPixel(x, y));

                // face
                for (var y = 0; y < 16; y++)
                for (var x = 0; x < 16; x++)
                    atlasTexture.SetPixel(x + index * 16, y, blueprint.face.GetPixel(x, y));

                index++;
            }
        }

        atlasTexture.filterMode = FilterMode.Point;
        atlasTexture.Apply();
    }

    private void RefreshPerlinTexture()
    {
        perlinTexture = new Texture2D(perlinWidth, perlinHeight);

        for (var x = 0; x < perlinWidth; x++)
        {
            for (var y = 0; y < perlinHeight; y++)
            {
                var xCoord = (float)x / perlinWidth * perlinScale + perlinOffsetX;
                var yCoord = (float)y / perlinHeight * perlinScale + perlinOffsetY;
                var sample = Mathf.PerlinNoise(xCoord, yCoord);
                var color = new Color(sample, sample, sample);
                perlinTexture.SetPixel(x, y, color);
            }
        }

        perlinTexture.Apply();
    }

    public float GetPerlinPosition(int x, int y)
    {
        x %= perlinWidth;
        y %= perlinHeight;
        if (x < 0) x = perlinWidth - x;
        if (y < 0) y = perlinHeight - y;
        return perlinTexture.GetPixel(x, y).r;
    }
}