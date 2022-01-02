using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bardo/Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    public List<TileSet> sets;

    public int defaultFloorIndex;
    public int defaultWallIndex;

    public TileSet DefaultFloor => sets[defaultFloorIndex];
    public TileSet DefaultWall => sets[defaultWallIndex];
}