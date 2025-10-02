using System;
using Unity.Burst.Intrinsics;
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
            _cameraInputService.OnCameraRotate += RotateCamera;
            _cameraInputService.OnCameraMove += MoveCamera;
        }


        public void Dispose()
        {
            _tileInputService.OnTileClicked -= TileClickedHandler;
            _cameraInputService.OnCameraRotate -= RotateCamera;
            _cameraInputService.OnCameraMove -= MoveCamera;
        }

        private void TileClickedHandler(WorldTile obj)
        {
            if(obj == null) return;
            var objectTransform = obj.transform;
            _cameraManager.LookAt(objectTransform);
        }

        private void RotateCamera(float input)
        {
            _cameraManager.Rotate(input);
        }

        private void MoveCamera(Vector2 move)
        {
            _cameraManager.Move(move);
        }
    }
}