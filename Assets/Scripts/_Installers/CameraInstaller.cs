using SurviveProject;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private CinemachineCamera _camera;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_cameraPivot).WithId("CameraPivot");

        Container.Bind<CameraManager>().AsSingle().WithArguments(_camera, _cameraPivot).NonLazy();
    }
}