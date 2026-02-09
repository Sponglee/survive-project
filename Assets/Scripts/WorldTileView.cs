using System;
using UnityEngine;

public class WorldTileView : MonoBehaviour, IDisposable
{
    [SerializeField] private OutlineIndicatorView outlineIndicatorView;
    [SerializeField] private Transform mapContentHolder;
    [SerializeField] private Transform buildingHolder;

    private int _id;
    private Vector2Int _coords;
    
    public Vector2Int TileCoords => _coords;
    public int Id => _id;
    
    public Transform ContentHolder => mapContentHolder;
    public Transform BuildingHolder => buildingHolder;

    public OutlineIndicatorView IndicatorView => outlineIndicatorView;

    public void SetId(int id, Vector2Int coords)
    {
        _id = id;
        _coords = coords;
    }
    
    public void Dispose()
    {
       
    }
}