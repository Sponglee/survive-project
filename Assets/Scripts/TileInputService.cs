using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TileInputService 
{
    public event Action<WorldTile> OnTileClicked;
    
    private WorldTile _currentSelectedTile = null;
    private InputAction _clickAction;
    
    private Camera _camera;
    private LayerMask _raycastLayerMask;

    public TileInputService(
        InputActionAsset inputActions,
        CameraManager cameraManager)
    {
        var map = inputActions.FindActionMap("Map");
        _clickAction = map.FindAction("MouseClick");
        _raycastLayerMask = 1 << LayerMask.NameToLayer("WorldTiles");
        _camera = cameraManager.MainCamera;
    }

    public void RaycastSelect()
    {
        if (!_clickAction.WasPressedThisFrame())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
            
        var mousePos = Mouse.current.position.ReadValue();
        var ray = _camera.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, _raycastLayerMask))
        {
            return;
        }
            
        if (hit.collider.TryGetComponent<WorldTile>(out var tile))
        {
            NotifyTileClicked(tile);
        }
    }
    
    private void NotifyTileClicked(WorldTile tile)
    {
        var tileToSelect = tile;
        if (_currentSelectedTile == tile)
        {
            _currentSelectedTile = null;
            tileToSelect = null;
        }
        _currentSelectedTile = tileToSelect;
        OnTileClicked?.Invoke(tileToSelect);
    }
}