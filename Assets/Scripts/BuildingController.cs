using System;
using TMPro;
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

    public void Dispose()
    {
        _model = null;
        GameObject.Destroy(_view.gameObject);
    }
}