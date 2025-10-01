using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelSpawnInstaller : MonoInstaller
{
    [SerializeField] private Transform _levelSpawnPointsHolder;

    public override void InstallBindings()
    {
        Container.BindInstance(_levelSpawnPointsHolder).WithId("WorldSpawn");
    }
}