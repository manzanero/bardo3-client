using UnityEngine;

public class Chunk : MonoBehaviour
{
    public bool dirty;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public readonly Tile[,] tiles = new Tile[World.ChunkSize, World.ChunkSize];
    public Wall[,] wallsH = new Wall[World.ChunkSize, World.ChunkSize];
    public Wall[,] wallsV = new Wall[World.ChunkSize, World.ChunkSize];

    public int initX;
    public int initY;

    private float unitUvTileX;
    private float unitUvTileY;

    private const float U = 1 / 16f;
    private const int TilePoints = 20;
    private const int TileTriangles = 10;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        for (var y = 0; y < World.ChunkSize; y++)
        for (var x = 0; x < World.ChunkSize; x++)
        {
            tiles[x, y] = Instantiate(World.instance.tilePrefab, new Vector3(x, 0, y), 
                Quaternion.identity, transform).GetComponent<Tile>();
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        if (dirty) Refresh();
    }

    private void Refresh()
    {
        RefreshMesh();
        dirty = false;
    }

    private void RefreshMesh()
    {
        var tileAtlasTexture = TileManager.instance.atlasTexture;
        meshRenderer.material.mainTexture = tileAtlasTexture;
        unitUvTileX = 16f / tileAtlasTexture.width;
        unitUvTileY = 16f / tileAtlasTexture.height;

        vertices = new Vector3[TilePoints * World.ChunkSize * World.ChunkSize];
        uv = new Vector2[TilePoints * World.ChunkSize * World.ChunkSize];
        triangles = new int[TileTriangles * 3 * World.ChunkSize * World.ChunkSize];

        for (var y = 0; y < World.ChunkSize; y++)
        for (var x = 0; x < World.ChunkSize; x++)
            RefreshTileMesh(x, y);

        var mesh = meshFilter.mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.Optimize();
    }

    private void RefreshTileMesh(int x, int y)
    {
        var tile = tiles[x, y];
        var h = tile.height * U;
        var offset = x + y * World.ChunkSize;
        var verticesOffset = offset * TilePoints;
        
        // top
        vertices[0 +  verticesOffset] = new Vector3(x, h, y);
        vertices[1 +  verticesOffset] = new Vector3(x, h, y + 1);
        vertices[2 +  verticesOffset] = new Vector3(x + 1, h, y);
        vertices[3 +  verticesOffset] = new Vector3(x + 1, h, y + 1);
        // south
        vertices[4 +  verticesOffset] = new Vector3(x, h - 1, y);
        vertices[5 +  verticesOffset] = new Vector3(x, h, y);
        vertices[6 +  verticesOffset] = new Vector3(x + 1, h - 1, y);
        vertices[7 +  verticesOffset] = new Vector3(x + 1, h, y);
        // north
        vertices[8 +  verticesOffset] = new Vector3(x + 1, h - 1, y + 1);
        vertices[9 +  verticesOffset] = new Vector3(x + 1, h, y + 1);
        vertices[10 + verticesOffset] = new Vector3(x, h - 1, y + 1);
        vertices[11 + verticesOffset] = new Vector3(x, h, y + 1);
        // west
        vertices[12 + verticesOffset] = new Vector3(x, h - 1, y + 1);
        vertices[13 + verticesOffset] = new Vector3(x, h, y + 1);
        vertices[14 + verticesOffset] = new Vector3(x, h - 1, y);
        vertices[15 + verticesOffset] = new Vector3(x, h, y);
        // est
        vertices[16 + verticesOffset] = new Vector3(x + 1, h - 1, y);
        vertices[17 + verticesOffset] = new Vector3(x + 1, h, y);
        vertices[18 + verticesOffset] = new Vector3(x + 1, h - 1, y + 1);
        vertices[19 + verticesOffset] = new Vector3(x + 1, h, y + 1);

        // top
        var uvPointsTop = GetUvPoints(tile.topIndex, 1);
        uv[0 + verticesOffset] = uvPointsTop[0];
        uv[1 + verticesOffset] = uvPointsTop[1];
        uv[2 + verticesOffset] = uvPointsTop[2];
        uv[3 + verticesOffset] = uvPointsTop[3];
        // south
        var uvPointsSouth = GetUvPoints(tile.southIndex, 0);
        uv[4 + verticesOffset] = uvPointsSouth[0];
        uv[5 + verticesOffset] = uvPointsSouth[1];
        uv[6 + verticesOffset] = uvPointsSouth[2];
        uv[7 + verticesOffset] = uvPointsSouth[3];
        // north
        var uvPointsNorth = GetUvPoints(tile.northIndex, 0);
        uv[8 + verticesOffset] = uvPointsNorth[0];
        uv[9 + verticesOffset] = uvPointsNorth[1];
        uv[10 + verticesOffset] = uvPointsNorth[2];
        uv[11 + verticesOffset] = uvPointsNorth[3];
        // west
        var uvPointsWest = GetUvPoints(tile.westIndex, 0);
        uv[12 + verticesOffset] = uvPointsWest[0];
        uv[13 + verticesOffset] = uvPointsWest[1];
        uv[14 + verticesOffset] = uvPointsWest[2];
        uv[15 + verticesOffset] = uvPointsWest[3];
        // est
        var uvPointsEst = GetUvPoints(tile.estIndex, 0);
        uv[16 + verticesOffset] = uvPointsEst[0];
        uv[17 + verticesOffset] = uvPointsEst[1];
        uv[18 + verticesOffset] = uvPointsEst[2];
        uv[19 + verticesOffset] = uvPointsEst[3];

        var trianglesOffset = offset * TileTriangles * 3;

        // top
        triangles[0 + trianglesOffset] = 0 + verticesOffset;
        triangles[1 + trianglesOffset] = triangles[4 + trianglesOffset] = 1 + verticesOffset;
        triangles[2 + trianglesOffset] = triangles[3 + trianglesOffset] = 2 + verticesOffset;
        triangles[5 + trianglesOffset] = 3 + verticesOffset;
        // S
        triangles[6 + trianglesOffset] = 4 + verticesOffset;
        triangles[7 + trianglesOffset] = triangles[10 + trianglesOffset] = 5 + verticesOffset;
        triangles[8 + trianglesOffset] = triangles[9 + trianglesOffset] = 6 + verticesOffset;
        triangles[11 + trianglesOffset] = 7 + verticesOffset;
        // W
        triangles[12 + trianglesOffset] = 8 + verticesOffset;
        triangles[13 + trianglesOffset] = triangles[16 + trianglesOffset] = 9 + verticesOffset;
        triangles[14 + trianglesOffset] = triangles[15 + trianglesOffset] = 10 + verticesOffset;
        triangles[17 + trianglesOffset] = 11 + verticesOffset;
        // N
        triangles[18 + trianglesOffset] = 12 + verticesOffset;
        triangles[19 + trianglesOffset] = triangles[22 + trianglesOffset] = 13 + verticesOffset;
        triangles[20 + trianglesOffset] = triangles[21 + trianglesOffset] = 14 + verticesOffset;
        triangles[23 + trianglesOffset] = 15 + verticesOffset;
        // E
        triangles[24 + trianglesOffset] = 16 + verticesOffset;
        triangles[25 + trianglesOffset] = triangles[28 + trianglesOffset] = 17 + verticesOffset;
        triangles[26 + trianglesOffset] = triangles[27 + trianglesOffset] = 18 + verticesOffset;
        triangles[29 + trianglesOffset] = 19 + verticesOffset;
    }

    private Vector2[] GetUvPoints(int index, int row)
    {
        var initUvX = index * unitUvTileX;
        var initUvY = row * unitUvTileY;

        return new[]
        {
            new Vector2(initUvX, initUvY),
            new Vector2(initUvX, initUvY + unitUvTileY),
            new Vector2(initUvX + unitUvTileX, initUvY),
            new Vector2(initUvX + unitUvTileX, initUvY + unitUvTileY)
        };
    }
}