using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class TileInputService : ITickable
{
    public event Action<WorldTile> OnTileClicked;
    
    private WorldTile _currentSelectedTile = null;
    private InputAction _clickAction;
    private Camera _camera;

    private LayerMask _raycastLayerMask;
    public TileInputService(CameraManager cameraManager, InputActionAsset inputActions)
    {
        _camera = cameraManager.MainCamera;
        var map = inputActions.FindActionMap("Map");
        _clickAction = map.FindAction("MouseClick");
        _raycastLayerMask = 1 << LayerMask.NameToLayer("WorldTiles");
    }
    
    public void Tick()
    {
        if (_clickAction.WasPressedThisFrame())
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            var mousePos = Mouse.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(mousePos);
            
            if (Physics.Raycast(ray, out RaycastHit hit,100f, _raycastLayerMask))
            {
                if (hit.collider.TryGetComponent<WorldTile>(out var tile))
                {
                    NotifyTileClicked(tile);
                }
            }
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