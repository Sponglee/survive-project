using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DefaultNamespace
{
    public class CameraInputService : IInputService, IInitializable, ITickable, IDisposable
    {
        public Vector2 CameraMoveInput { get; private set; }
        public float CameraRotateInput { get; private set; }
        
        public event Action<Vector2> OnCameraMove;
        public event Action<float> OnCameraRotate;
        
        private InputAction _moveAction;
        private InputAction _rotateAction;

        private bool _isCameraMoving;
        
        public CameraInputService(InputActionAsset input)
        {
            var map = input.FindActionMap("Map");
                map.Enable();
            _moveAction = map.FindAction("Move");
            _rotateAction = map.FindAction("CameraRotate");
        }
        
        public void Initialize()
        {
            _moveAction.started += ctx => _isCameraMoving = true;
            _moveAction.canceled += ctx => _isCameraMoving = false;

            _rotateAction.performed += ctx => OnCameraRotate?.Invoke(CameraRotateInput);
            
            _moveAction.Enable();
            _rotateAction.Enable();
        }
        
        public void Tick()
        {
            CameraMoveInput = _moveAction.ReadValue<Vector2>();
            CameraRotateInput = _rotateAction.ReadValue<float>();

            if (_isCameraMoving)
            {
                OnCameraMove?.Invoke(CameraMoveInput);
            }
        }
        
        public void Dispose()
        {
            _moveAction?.Disable();
            _rotateAction?.Disable();
            _moveAction?.Dispose();
        }
    }
}