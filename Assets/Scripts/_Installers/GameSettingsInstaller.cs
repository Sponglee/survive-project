using SurviveProject;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Scriptable Objects/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CameraPreset _cameraSettingsPreset;
    [SerializeField] private AudioPreset _audioSettings;
    [SerializeField] private UIElementsPreset _uiElementsPreset;

    public override void InstallBindings()
    {
        Container.Bind<CameraPreset>().FromInstance(_cameraSettingsPreset).AsSingle().NonLazy();
        Container.Bind<AudioPreset>().FromInstance(_audioSettings).AsSingle().NonLazy();
        Container.Bind<UIElementsPreset>().FromInstance(_uiElementsPreset).AsSingle().NonLazy();
        
        Container.Bind<BuildingMenuItemFactory>().AsSingle().NonLazy();

    }
}
