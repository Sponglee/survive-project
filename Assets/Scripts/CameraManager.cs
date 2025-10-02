using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CameraManager
    {
        private const float CAMERA_MOVE_SPEED = .5f;
        private const float CAMERA_ROTATE_SPEED = 1f;
        private const Ease CAMERA_LOOKAT_EASE = Ease.OutCubic;
        private const float CAMERA_LOOKAT_DURATION = 1f;
        
        
        private CinemachineCamera _camera;
        private CinemachineOrbitalFollow _orbitalFollow;
        private Transform _cameraPivot;
        
        private CameraState _cameraState;

        [Inject]
        public CameraManager(
            CinemachineCamera camera,
            [Inject(Id = "CameraPivot")] Transform cameraPivot)
        {
            _cameraPivot = cameraPivot;
            _camera = camera;
            _orbitalFollow = _camera.GetComponent<CinemachineOrbitalFollow>();
        }


        public void LookAt(Transform objTransform)
        {
            ChangeState(CameraState.Cinematic);
            var lookAtPos = Vector3.Scale(new Vector3(1,0,1),objTransform.position);
            _cameraPivot.DOMove(lookAtPos, CAMERA_LOOKAT_DURATION).SetEase(CAMERA_LOOKAT_EASE).OnComplete(()=>ChangeState(CameraState.LookAt));
        }

        private void ChangeState(CameraState targetState)
        {
            if (_cameraState == targetState)
            {
                return;
            }
            
            _cameraState = targetState;
        }

        public void Rotate(float input)
        {
            _orbitalFollow.HorizontalAxis.Value += input * CAMERA_ROTATE_SPEED;
            _orbitalFollow.HorizontalAxis.Validate();
        }

        public void Move(Vector2 move)
        {
            if (_cameraState == CameraState.Cinematic)
            {
                return;
            }

            var moveDirection = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0)
                                * new Vector3(move.x, 0, move.y);
            _cameraPivot.Translate(moveDirection * CAMERA_MOVE_SPEED);
        }
    }

    internal enum CameraState
    {
        LookAt,
        Cinematic
    }
}