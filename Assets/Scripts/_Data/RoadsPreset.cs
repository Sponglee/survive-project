using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "RoadsPreset", menuName = "Scriptable Objects/RoadsPreset")]
public class RoadsPreset : ScriptableObject
{
    public List<RoadData> RoadsList = new List<RoadData>();
}

[Serializable]
public class RoadData
{
    public GameObject RoadElementPrefab;
}
