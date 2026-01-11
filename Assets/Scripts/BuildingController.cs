using System;
using DG.Tweening;
using UnityEngine;

public class BuildingController : IDisposable
{
    private BuildingModel _model;
    private BuildingView _view;
    
    public BuildingData Data => _model.BuildingData;
    public BuildingView View => _view;

    private Tween _rotationTween; 
    
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
        _rotationTween?.Kill();
        _rotationTween = null;
        
        if (_view != null && _view.gameObject != null)
        {
            GameObject.Destroy(_view.gameObject);
        }
        
        _model = null;
    }

    public void Rotate(float angle)
    {
        _rotationTween?.Kill();
        _rotationTween = null;
    
        var currentY = _view.transform.eulerAngles.y;
        var newY = currentY + angle;
    
        var snappedY = Mathf.Round(newY / 90f) * 90f;
    
       _rotationTween = _view.transform.DORotate(new Vector3(0f, snappedY, 0f), 0.5f)
            .SetEase(Ease.OutQuad);
    }
}