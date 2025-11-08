using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsPreset", menuName = "Scriptable Objects/BuildingsPreset")]
public class BuildingsPreset : ScriptableObject
{
    public List<BuildingData> BuildingsList = new List<BuildingData>();
}

[Serializable]
public class BuildingData
{
    public string Name;
    public GameObject Prefab;
}