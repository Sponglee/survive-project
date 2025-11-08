using SurviveProject;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputSystemInstaller : MonoInstaller<InputSystemInstaller>
{
    [SerializeField] private InputActionAsset _inputAsset;

    public override void InstallBindings()
    {
        Container.Bind<InputActionAsset>()
            .FromInstance(_inputAsset)
            .AsSingle();

        Container.BindInterfacesAndSelfTo<CameraInputProvider>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraInputService>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<PlayerInputProvider>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerInputService>().AsSingle().NonLazy();

    }
}
