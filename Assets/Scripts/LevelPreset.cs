using System.Collections.Generic;
using ModestTree.Util;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPreset", menuName = "Scriptable Objects/LevelPreset")]
public class LevelPreset : ScriptableObject
{
    public float tileSize;
    public GameObject[] tileList;
    [TextArea(20,20)]
    public string map;

    [HideInInspector]
    public ValuePair<int, int> LevelSize => GetMapSize();

    private ValuePair<int, int> GetMapSize()
    {
        var mapRows = map.Split("\n");
        return new ValuePair<int, int>(mapRows[0].Length, mapRows.Length);
    }
}

