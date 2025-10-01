using System.Collections.Generic;
using ModestTree.Util;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPreset", menuName = "Scriptable Objects/LevelPreset")]
public class WorldPreset : ScriptableObject
{
    public float TileSize;
    public GameObject[] TileList;
    [TextArea(20,20)]
    public string Map;

    [HideInInspector]
    public ValuePair<int, int> LevelSize => GetMapSize();

    private ValuePair<int, int> GetMapSize()
    {
        var mapRows = Map.Split("\n");
        return new ValuePair<int, int>(mapRows[0].Length, mapRows.Length);
    }
}

