using System;
using SurviveProject;
using UnityEngine;
using Zenject;

public class WorldTileView : MonoBehaviour, IDisposable
{
    [SerializeField] private OutlineIndicatorView _outlineIndicatorView;
    [SerializeField] private Transform _mapContentHolder;
    [SerializeField] private Transform _buildingHolder;

    private int _id;

    public int Id => _id;
    
    public Transform ContentHolder => _mapContentHolder;
    public Transform BuildingHolder => _buildingHolder;

    public OutlineIndicatorView IndicatorView => _outlineIndicatorView;

    public void SetId(int id)
    {
        _id = id;
    }
    
    public void Dispose()
    {
       
    }
}