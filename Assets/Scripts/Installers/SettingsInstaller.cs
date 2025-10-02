using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Scriptable Objects/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CameraSettingsPreset _cameraSettingsPreset;
    
    public override void InstallBindings()
    {
        Container.Bind<CameraSettingsPreset>().FromInstance(_cameraSettingsPreset).AsSingle();
    }
}
