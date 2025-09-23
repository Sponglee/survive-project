using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelSpawnInstaller : MonoInstaller
{
    [SerializeField] private Transform _levelSpawnPointsHolder;
    [SerializeField] private LevelPreset _levelPreset;
    
    public override void InstallBindings()
    {
        BindTile();
    }

    private void BindTile()
    {
        for (var x = 0; x < _levelPreset.LevelSize.First; x++)
        {
            for (var y = 0; y < _levelPreset.LevelSize.Second; y++)
            {
                var hasTile = GetTilePrefab(x, y, out var tilePrefab);
                if (!hasTile)
                    continue;
                
                var tile = Container.InstantiatePrefabForComponent<ITile>(tilePrefab, GetSpawnPosition(x,y, _levelPreset.tileSize), Quaternion.identity, _levelSpawnPointsHolder);
                Container.Bind<ITile>().FromInstance(tile);
            }
        }
    }

    private Vector3 GetSpawnPosition(int tileX, int tileY, float tileSize)
    {
        var position = new Vector3(tileX * tileSize, 0, tileY * tileSize) - new Vector3(_levelPreset.LevelSize.First/2, 0f, _levelPreset.LevelSize.Second/2) * tileSize;

        return position;
    }

    private bool GetTilePrefab(int x, int y, out GameObject tilePrefab)
    {
        tilePrefab = null;
        var map = _levelPreset.map;
        var mapRows = map.Split("\n");

        if (mapRows[y].Length < x)
        {
            return false;
        }
        
        var tileData = mapRows[y][x].ToString();
       var parseSuccess = int.TryParse(tileData, out int mapTileData);

        if (!parseSuccess || _levelPreset.tileList.Length < mapTileData)
        {
            return false;
        }
        tilePrefab = _levelPreset.tileList[mapTileData];
        
        return  tilePrefab != null;
    }
}