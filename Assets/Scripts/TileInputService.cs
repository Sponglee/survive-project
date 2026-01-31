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
    private readonly InputAction _clickActionHold;

    private readonly Camera _camera;
    private readonly LayerMask _rayCastLayerMask;
    private WorldTileView _lastRaycastTile;
    
    private Action<InputAction.CallbackContext> _clickHoldHandler;
    public event Action<bool> OnClickActionHold;
    public WorldTileView LastRayCastTile => _lastRaycastTile;
    
    public TileInputService(
        InputActionAsset inputActions,
        CameraManager cameraManager)
    {
        var map = inputActions.FindActionMap("Map");
        map.Enable(); 
        _clickAction = map.FindAction("MouseClick");
        _clickActionHold = map.FindAction("MouseClickHold");

        _clickAction.Enable();
        _clickActionHold.Enable();
        
        _clickActionHold.started += ctx =>
        {
            OnClickActionHold?.Invoke(true);
        };
        _clickActionHold.canceled += ctx =>  OnClickActionHold?.Invoke(false);;
        
        _rayCastLayerMask = 1 << LayerMask.NameToLayer("WorldTiles");
        _camera = cameraManager.MainCamera;
    }
    
    public void Dispose()
    {
        _clickAction?.Disable();
        _clickActionHold?.Disable();
        
        _clickAction?.Dispose();
        _clickActionHold?.Dispose();
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
            if (_lastRaycastTile != null)
            {
                NotifyTileHover(null);
            }
            return;
        }
        
        if (Mouse.current == null)
        {
            return;
        }
            
        var mousePos = Mouse.current.position.ReadValue();
        var ray = _camera.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out var hit, 100f, _rayCastLayerMask))
        {
            if (_lastRaycastTile != null)
            {
                NotifyTileHover(null);
            }
            return;
        }
            
        if (!hit.collider.TryGetComponent(out WorldTileView tile))
        { 
            if (_lastRaycastTile != null)
            {
                NotifyTileHover(null);
            }
            return;   
        }

        if (_lastRaycastTile != tile)
        {
            NotifyTileHover(tile);
        }
        
        if (!_clickAction.WasPerformedThisFrame())
        {
            return;
        }
        
        NotifyTileClicked(tile);
    }

    private void NotifyTileHover(WorldTileView tileView)
    {
        _lastRaycastTile = tileView;
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