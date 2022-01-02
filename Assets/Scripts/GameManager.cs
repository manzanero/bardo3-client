using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState GameState;
    
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.LoadWorld);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.LoadWorld:
                ChangeState(GameState.LoadScene);
                break;
            case GameState.LoadScene:
                TileManager.instance.RefreshTileAtlasTexture();
                SceneGenerator.instance.CreateSceneFromDonjonJson("Maps/Donjon_Small");
                ChangeState(GameState.ShareActions);
                break;
            case GameState.ShareActions:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}
    
public enum GameState
{
    LoadWorld = 0,
    LoadScene = 1,
    ShareActions = 2,
}