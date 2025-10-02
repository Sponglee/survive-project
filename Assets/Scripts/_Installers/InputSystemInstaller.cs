using DefaultNamespace;
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

        Container.BindInterfacesAndSelfTo<CameraInputService>().AsSingle().NonLazy();
    }
}
