using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;


namespace SurviveProject
{
    public class TileInputProvider : IInitializable, ITickable, IDisposable
    {
        private readonly BuildingService _buildingService;
        private readonly TileManager _tileManager;
        private readonly TileInputService _tileInputService;
        private readonly CameraManager _cameraManager;

        private bool _canSelectTile = true;

        public TileInputProvider(
            TileInputService tileInputService,
            BuildingService buildingService,
            TileManager tileManager,
            CameraManager cameraManager)

        {
            _tileInputService = tileInputService;
            _buildingService = buildingService;
            _tileManager = tileManager;
            _cameraManager = cameraManager;
        }
        
        public void Initialize()
        {
            _tileInputService.OnTileClicked += TileClickedHandler;
        }

        public void Dispose()
        {
            _tileInputService.OnTileClicked -= TileClickedHandler;
        }
        
        public void Tick()
        {
            if (!_canSelectTile)
            {
                return;
            }
            
            _tileInputService.RaycastSelect();
        }
        
        private void TileClickedHandler(WorldTileView obj)
        {
            if(obj == null) return;

            var tile = _tileManager.GetTileByView(obj);

            if (tile.IsEmpty)
            {
                return;
            }

            if (_buildingService.IsBuildingInProgress)
            {
                return;
            }
            
            _cameraManager.LookAt(obj.transform);
        }
    }
}