using UnityEngine;

public class World : MonoBehaviour
{
    public bool debugging;
    
    public GameObject chunkPrefab;
    public GameObject tilePrefab;
    public GameObject edgePrefab;
    
    public Scene scene;
    
    public const int ChunkSize = 16;

    public static World instance;

    private void Awake()
    {
        instance = this;
    }
    
    
}