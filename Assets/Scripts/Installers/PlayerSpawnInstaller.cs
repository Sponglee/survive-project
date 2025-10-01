using UnityEngine;
using Zenject;

public class PlayerSpawnInstaller : MonoInstaller
{
    [SerializeField] private Transform _playerSpawnPoint;

    public override void InstallBindings()
    {
        Container.BindInstance(_playerSpawnPoint).WithId("PlayerSpawn");
    }
}