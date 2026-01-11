using System;
using SurviveProject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TileInputService : IDisposable
{
    public event Action<WorldTileView> OnTileClicked;
    public event Action<WorldTileView> OnTileHover;

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

    public void SelectTile(WorldTileView view)
    {
        _currentSelectedTileView = view;
        OnTileClicked?.Invoke(view);
    }
    
    public void RaycastSelect()
    {
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
            
        if (!hit.collider.TryGetComponent(out WorldTileView tile))
        {
           return;   
        }

        NotifyTileHover(tile);
        
        if (!_clickAction.WasPerformedThisFrame())
        {
            return;
        }
        
        NotifyTileClicked(tile);
    }

    private void NotifyTileHover(WorldTileView tileView)
    {
        OnTileHover?.Invoke(tileView);
    }

    private void NotifyTileClicked(WorldTileView tileView)
    {
        if (_currentSelectedTileView == tileView)
        {
           SelectTile(null);
        }
        else
        {
           SelectTile(tileView);
        }
    }
}