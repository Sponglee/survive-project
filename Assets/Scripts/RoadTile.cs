using System.Collections.Generic;
using UnityEngine;

public class RoadTile
{
    public ITile Tile { get; private set; }
    public BuildingController RoadController { get; private set; }
    public Dictionary<Vector2Int, RoadElementView> Connections { get; private set; }
    
    public RoadTile(ITile tile, BuildingController roadController)
    {
        Tile = tile;
        RoadController = roadController;
        Connections = new Dictionary<Vector2Int, RoadElementView>();
    }
    
    public void AddConnection(Vector2Int targetCoords, RoadElementView roadElement)
    {
        if (!Connections.ContainsKey(targetCoords))
        {
            Connections[targetCoords] = roadElement;
        }
    }
    
    public void RemoveConnection(Vector2Int targetCoords)
    {
        if (Connections.TryGetValue(targetCoords, out var roadElement))
        {
            if (roadElement != null)
            {
                Object.Destroy(roadElement.gameObject);
            }
            Connections.Remove(targetCoords);
        }
    }
    
    public bool IsConnectedTo(Vector2Int coords)
    {
        return Connections.ContainsKey(coords);
    }
    
    public RoadElementView GetConnection(Vector2Int coords)
    {
        Connections.TryGetValue(coords, out var element);
        return element;
    }
    
    public int ConnectionCount => Connections.Count;
    
    public Transform GetConnectionPoint()
    {
        var roadView = RoadController?.View as RoadView;
        return roadView?.RoadConnectionPoint;
    }
}