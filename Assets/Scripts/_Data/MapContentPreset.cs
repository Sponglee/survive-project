using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapContentPreset", menuName = "Scriptable Objects/MapContentPreset")]
public class MapContentPreset : ScriptableObject
{
    public List<MapContentPresetData> MapContentList = new List<MapContentPresetData>();
}

[Serializable]
public class MapContentPresetData
{
    public MapContentType Type;
    public GameObject Prefab;
}

public enum MapContentType
{
   Resource,
   Obstacle
}