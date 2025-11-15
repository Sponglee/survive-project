using System;
using UnityEngine;

public class WorldTileView : MonoBehaviour, IDisposable
{
    [SerializeField] private OutlineIndicatorView outlineIndicatorView;
    [SerializeField] private Transform mapContentHolder;
    [SerializeField] private Transform buildingHolder;

    private int _id;

    public int Id => _id;
    
    public Transform ContentHolder => mapContentHolder;
    public Transform BuildingHolder => buildingHolder;

    public OutlineIndicatorView IndicatorView => outlineIndicatorView;

    public void SetId(int id)
    {
        _id = id;
    }
    
    public void Dispose()
    {
       
    }
}