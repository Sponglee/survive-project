using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;


namespace DefaultNamespace
{
    public class TileInputProvider : IInitializable, ITickable, IDisposable
    {
        private TileInputService _tileInputService;
        private CameraManager _cameraManager;

        private bool _canSelectTile = true;
        
        public TileInputProvider(
            TileInputService tileInputService,
            CameraManager cameraManager)

        {
            _tileInputService = tileInputService;
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
        
        private void TileClickedHandler(WorldTile obj)
        {
            if(obj == null) return;
            var objectTransform = obj.transform;
            _cameraManager.LookAt(objectTransform);
        }
    }
}