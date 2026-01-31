using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SurviveProject
{
    public class CameraInputService : IInitializable, ITickable, IDisposable
    {
        private Vector2 CameraMoveInput { get; set; }
        private Vector2 CameraPanInput { get; set; }
        private float CameraRotateInput { get; set; }
        private float CameraZoomInput { get; set; }
        public bool IsZoomLocked => _isZoomLocked;
        
        public event Action<Vector2> OnCameraMove;
        public event Action<Vector2> OnCameraPan;
        public event Action<float> OnCameraRotate;
        public event Action<float> OnCameraZoom; 

        private readonly InputAction _moveAction;
        private readonly InputAction _rotateAction;
        private readonly InputAction _zoomAction;
        private readonly InputAction _panAction;
        
        private bool _isCameraMoving;
        private bool _isCameraPanning;
        private bool _isCameraZooming;
        private bool _isCameraRotating;
        
        private bool _isZoomLocked = false;
        
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

            _rotateAction.performed += ctx => _isCameraRotating = true;
            _rotateAction.canceled += ctx => _isCameraRotating = false;

            _moveAction.Enable();
            _rotateAction.Enable();
            _zoomAction.Enable();
        }
        
        public void Dispose()
        {
            _moveAction?.Disable();
            _rotateAction?.Disable();
            _zoomAction?.Disable();
            _rotateAction?.Disable();
            
            _moveAction?.Dispose();
            _rotateAction?.Dispose();
            _zoomAction?.Dispose();
            _rotateAction?.Dispose();
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

            if (_isCameraRotating)
            {
                OnCameraRotate?.Invoke(CameraRotateInput);
            }
        }

        public void SetIsZoomLocked(bool toggle)
        {
            _isZoomLocked = toggle;
        }
    }
}