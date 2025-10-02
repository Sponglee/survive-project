using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CameraInputProvider : IInitializable, IDisposable
    {

        
        private TileInputService _tileInputService;
        private CameraInputService _cameraInputService;
        private CameraManager _cameraManager;

        [Inject]
        public CameraInputProvider(
            TileInputService tileInputService,
            CameraInputService cameraInputService,
            CameraManager cameraManager)
        {
            _cameraInputService = cameraInputService;
            _tileInputService = tileInputService;
            _cameraManager = cameraManager;
        }


        public void Initialize()
        {
            _tileInputService.OnTileClicked += TileClickedHandler;
            _cameraInputService.OnCameraMove += MoveCamera;
            _cameraInputService.OnCameraPan += PanCamera;
            _cameraInputService.OnCameraRotate += RotateCamera;
            _cameraInputService.OnCameraZoom += ZoomCamera;

        }


        public void Dispose()
        {
            _tileInputService.OnTileClicked -= TileClickedHandler;
            _cameraInputService.OnCameraMove -= MoveCamera;
            _cameraInputService.OnCameraPan -= PanCamera;
            _cameraInputService.OnCameraRotate -= RotateCamera;
            _cameraInputService.OnCameraZoom -= ZoomCamera;
        }

        private void TileClickedHandler(WorldTile obj)
        {
            if(obj == null) return;
            var objectTransform = obj.transform;
            _cameraManager.LookAt(objectTransform);
        }

        private void MoveCamera(Vector2 move)
        {
            _cameraManager.Move(move);
        }
        
        private void PanCamera(Vector2 move)
        {
            _cameraManager.Pan(move);
        }
        
        private void RotateCamera(float input)
        {
            _cameraManager.Rotate(input);
        }
        
        private void ZoomCamera(float zoom)
        {
            _cameraManager.Zoom(zoom);
        }
    }
}