using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bardo/Tile Set")]
public class TileSet : ScriptableObject
{
    public List<TileBlueprint> blueprints;
}