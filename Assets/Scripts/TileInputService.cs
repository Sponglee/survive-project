using System;
using SurviveProject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TileInputService : IDisposable
{
    public event Action<WorldTileView> OnTileClicked;
    
    private WorldTileView _currentSelectedTileView = null;
    private readonly InputAction _clickAction;
    
    private readonly Camera _camera;
    private readonly LayerMask _rayCastLayerMask;

    public TileInputService(
        InputActionAsset inputActions,
        CameraManager cameraManager)
    {
        var map = inputActions.FindActionMap("Map");
        map.Enable(); 
        _clickAction = map.FindAction("MouseClick");
        _clickAction.Enable();
        
        _rayCastLayerMask = 1 << LayerMask.NameToLayer("WorldTiles");
        _camera = cameraManager.MainCamera;
    }
    
    public void Dispose()
    {
        _clickAction?.Disable();
    }

    public void RaycastSelect()
    {
        if (!_clickAction.WasPressedThisFrame())
        {
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (Mouse.current == null)
        {
            return;
        }
            
        var mousePos = Mouse.current.position.ReadValue();
        var ray = _camera.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, _rayCastLayerMask))
        {
            return;
        }
            
        if (hit.collider.TryGetComponent(out WorldTileView tile))
        {
            NotifyTileClicked(tile);
        }
    }
    
    private void NotifyTileClicked(WorldTileView tileView)
    {
        if (_currentSelectedTileView == tileView)
        {
            _currentSelectedTileView = null;
            OnTileClicked?.Invoke(null);
        }
        else
        {
            _currentSelectedTileView = tileView;
            OnTileClicked?.Invoke(tileView);
        }
    }
}