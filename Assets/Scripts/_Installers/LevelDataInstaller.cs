using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LevelDataInstaller", menuName = "Scriptable Objects/LevelDataInstaller")]
public class LevelDataInstaller : ScriptableObjectInstaller
{
    public WorldPreset _worldPreset;
    public PlayerPreset _playerPreset;
    public MapContentPreset _mapContentPreset;
    public BuildingsPreset _buildingsPreset;
    
    public override void InstallBindings()
    {
        Container.Bind<WorldPreset>().FromInstance(_worldPreset).AsSingle();
        Container.Bind<PlayerPreset>().FromInstance(_playerPreset).AsSingle();
        Container.Bind<MapContentPreset>().FromInstance(_mapContentPreset).AsSingle();
        Container.Bind<BuildingsPreset>().FromInstance(_buildingsPreset).AsSingle();
    }
}
