using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "LevelDataInstaller", menuName = "Scriptable Objects/LevelDataInstaller")]
public class LevelDataInstaller : ScriptableObjectInstaller
{
    public WorldPreset WorldPreset;
    public PlayerPreset PlayerPreset;
    [FormerlySerializedAs("WorldObjectsPreset")] public MapContentPreset mapContentPreset;
    public BuildingsPreset BuildingsPreset;
    
    public override void InstallBindings()
    {
        Container.Bind<WorldPreset>().FromInstance(WorldPreset).AsSingle();
        Container.Bind<PlayerPreset>().FromInstance(PlayerPreset).AsSingle();
        Container.Bind<MapContentPreset>().FromInstance(mapContentPreset).AsSingle();
        Container.Bind<BuildingsPreset>().FromInstance(BuildingsPreset).AsSingle();
    }
}
