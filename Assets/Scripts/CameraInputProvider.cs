using System;
using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class CameraInputProvider : IInitializable, IDisposable
    {

        private CameraInputService _cameraInputService;
        private CameraManager _cameraManager;

        [Inject]
        public CameraInputProvider(
            CameraInputService cameraInputService,
            CameraManager cameraManager)
        {
            _cameraInputService = cameraInputService;
            _cameraManager = cameraManager;
        }


        public void Initialize()
        {
            _cameraInputService.OnCameraMove += MoveCamera;
            _cameraInputService.OnCameraPan += PanCamera;
            _cameraInputService.OnCameraRotate += RotateCamera;
            _cameraInputService.OnCameraZoom += ZoomCamera;
        }


        public void Dispose()
        {
            _cameraInputService.OnCameraMove -= MoveCamera;
            _cameraInputService.OnCameraPan -= PanCamera;
            _cameraInputService.OnCameraRotate -= RotateCamera;
            _cameraInputService.OnCameraZoom -= ZoomCamera;
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