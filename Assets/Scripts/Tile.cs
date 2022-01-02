using System;
using UnityEngine;
using static UnityEngine.Random;

public class Tile : MonoBehaviour
{
    public int topIndex;
    public int southIndex;
    public int northIndex;
    public int westIndex;
    public int estIndex;

    public int height;
    public bool transparent;

    public bool explored;
    public bool revealed;
    
    private TileManager _manager;

    private void Awake()
    {
        _manager = TileManager.instance;
    }

    public void SetTileSet(TileSet set, SetMethod method = SetMethod.Main)
    {
        var setIndex = _manager.atlasSetIndexes[set];
        switch (method)
        {
            case SetMethod.Main:
                topIndex = southIndex = northIndex = westIndex = estIndex = setIndex;
                break;
            case SetMethod.Random:
                var bps = set.blueprints.Count;
                topIndex = setIndex + Range(0, bps);
                southIndex = setIndex + Range(0, bps);
                northIndex = setIndex + Range(0, bps);
                westIndex = setIndex + Range(0, bps);
                estIndex = setIndex + Range(0, bps);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }
}

public enum SetMethod
{
    Main,
    Random,
}