using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CameraManager
    {
        public Camera MainCamera => _mainCamera ??= _camera.GetComponent<Camera>(); 
        
        private CameraPreset _cameraSettingsPreset;

        private Camera _mainCamera;
        private CinemachineCamera _camera;
        private CinemachineOrbitalFollow _orbitalFollow;
        private Transform _cameraPivot;
        private Tween _moveCameraTween;
        
        private CameraState _cameraState;
        public event Action<CameraState> CameraStateChanged; 
        
        [Inject]
        public CameraManager(
            CinemachineCamera camera,
            CameraPreset cameraSettings,
            [Inject(Id = "CameraPivot")] Transform cameraPivot)
        {
            _cameraPivot = cameraPivot;
            _cameraSettingsPreset = cameraSettings;
            _camera = camera;
            _orbitalFollow = _camera.GetComponent<CinemachineOrbitalFollow>();
        }


        public void LookAt(Transform objTransform)
        {
            _moveCameraTween?.Kill();
            _moveCameraTween = null;
            
            ChangeState(CameraState.Cinematic);
            var lookAtPos = Vector3.Scale(new Vector3(1,0,1),objTransform.position);
            _moveCameraTween = _cameraPivot.DOMove(lookAtPos, _cameraSettingsPreset.CameraLookAtDuration).SetEase(_cameraSettingsPreset.CameraLookAtEase).OnComplete(()=>ChangeState(CameraState.LookAt));
        }

        private void ChangeState(CameraState targetState)
        {
            if (_cameraState == targetState)
            {
                return;
            }
            
            _cameraState = targetState;
            CameraStateChanged?.Invoke(_cameraState);
        }
        
        public void Move(Vector2 move)
        {
           MoveCamera(move, _cameraSettingsPreset.CameraMoveSpeed);
        }
        
        public void Pan(Vector2 move)
        {
            MoveCamera(move, _cameraSettingsPreset.CameraPanSpeed);
        }

        public void Rotate(float input)
        {
            _orbitalFollow.HorizontalAxis.Value += input * _cameraSettingsPreset.CameraRotateSpeed;
            _orbitalFollow.HorizontalAxis.Validate();
        }
        
        public void Zoom(float zoom)
        {
            _orbitalFollow.RadialAxis.Value -= zoom * _cameraSettingsPreset.CameraZoomSpeed;
            _orbitalFollow.RadialAxis.Validate();
        }
        
        private void MoveCamera(Vector2 move, float speedOverride)
        {
            ChangeState(CameraState.Idle);
            
            _moveCameraTween?.Kill();
            _moveCameraTween = null;
            
            var moveDirection = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0)
                                * new Vector3(move.x, 0, move.y) * _orbitalFollow.RadialAxis.Value;
            _cameraPivot.Translate(moveDirection * speedOverride);
        }
    }

    public enum CameraState
    {
        LookAt,
        Cinematic,
        Idle
    }
}