using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SurviveProject
{
    public class CameraInputService : IInitializable, ITickable, IDisposable
    {
        public Vector2 CameraMoveInput { get; private set; }
        public Vector2 CameraPanInput { get; private set; }
        public float CameraRotateInput { get; private set; }
        public float CameraZoomInput { get; private set; }
        public event Action<Vector2> OnCameraMove;
        public event Action<Vector2> OnCameraPan;

        public event Action<float> OnCameraRotate;
        public event Action<float> OnCameraZoom; 

        private InputAction _moveAction;
        private InputAction _rotateAction;
        private InputAction _zoomAction;
        private InputAction _panAction;
        
        private bool _isCameraMoving;
        private bool _isCameraPanning;
        private bool _isCameraZooming;
        
        public CameraInputService(InputActionAsset input)
        {
            var map = input.FindActionMap("Map");
                map.Enable();
            _moveAction = map.FindAction("Move");
            _rotateAction = map.FindAction("CameraRotate");
            _zoomAction = map.FindAction("CameraZoom");
            _panAction = map.FindAction("CameraPan");
        }
        
        public void Initialize()
        {
            _moveAction.started += ctx => _isCameraMoving = true;
            _moveAction.canceled += ctx => _isCameraMoving = false;

            _panAction.started += ctx => _isCameraPanning = true;
            _panAction.canceled += ctx => _isCameraPanning = false;
            
            _zoomAction.started += ctx => _isCameraZooming = true;
            _zoomAction.canceled += ctx => _isCameraZooming = false;

            _rotateAction.performed += ctx => OnCameraRotate?.Invoke(CameraRotateInput);

            _moveAction.Enable();
            _rotateAction.Enable();
            _zoomAction.Enable();
        }
        
        public void Tick()
        {
            CameraMoveInput = _moveAction.ReadValue<Vector2>();
            CameraRotateInput = _rotateAction.ReadValue<float>();
            CameraZoomInput = _zoomAction.ReadValue<float>();
            CameraPanInput = -_panAction.ReadValue<Vector2>();
            
            if (_isCameraMoving && !_isCameraPanning)
            {
                OnCameraMove?.Invoke(CameraMoveInput);
            }
            
            if (_isCameraPanning && !_isCameraMoving)
            {
                OnCameraPan?.Invoke(CameraPanInput);
            }

            if (_isCameraZooming)
            {
                OnCameraZoom?.Invoke(CameraZoomInput);
            }
        }
        
        public void Dispose()
        {
            _moveAction?.Disable();
            _rotateAction?.Disable();
            _zoomAction?.Disable();
            
            _moveAction?.Dispose();
            _rotateAction?.Dispose();
            _zoomAction?.Dispose();

        }
    }
}