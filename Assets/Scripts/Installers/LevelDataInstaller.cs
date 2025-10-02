using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "LevelDataInstaller", menuName = "Scriptable Objects/LevelDataInstaller")]
public class LevelDataInstaller : ScriptableObjectInstaller
{
    public WorldPreset _worldPreset;
    public PlayerPreset PlayerPreset;
    
    public override void InstallBindings()
    {
        Container.Bind<WorldPreset>().FromInstance(_worldPreset).AsSingle();
        Container.Bind<PlayerPreset>().FromInstance(PlayerPreset).AsSingle();
    }
}
