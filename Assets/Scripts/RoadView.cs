using UnityEngine;

public class RoadView : BuildingView
{
    [SerializeField] private Transform roadConnectionPoint;
    
    public Transform RoadConnectionPoint => roadConnectionPoint;
}