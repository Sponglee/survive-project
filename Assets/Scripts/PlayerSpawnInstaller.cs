using UnityEngine;
using Zenject;

public class PlayerSpawnInstaller : MonoInstaller
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    public override void InstallBindings()
    {
        BindPlayer();
    }

    private void BindPlayer()
    {
        IPlayer player = Container.InstantiatePrefabForComponent<IPlayer>(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity, _playerSpawnPoint);

        Container.Bind<IPlayer>().FromInstance(player);
    }
}