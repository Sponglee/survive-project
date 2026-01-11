using System;
using UnityEngine;

public class BuildingController : IDisposable
{
    private BuildingModel _model;
    private BuildingView _view;
    
    public BuildingData Data => _model.BuildingData;
    public BuildingView View => _view;
    
    public BuildingController(BuildingView buildingView, BuildingModel buildingModel)
    {
        _model = buildingModel;
        _view = buildingView;
    }

    public void Initialize()
    {
        
    }
    
    public void Dispose()
    {
        if (_view != null && _view.gameObject != null)
        {
            GameObject.Destroy(_view.gameObject);
        }
        
        _model = null;
    }
}