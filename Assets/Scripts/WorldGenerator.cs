using DefaultNamespace;
using UnityEngine;
using Zenject;

public class WorldGenerator : IInitializable
{
    private ITileFactory _tileFactory;
    private WorldPreset _worldPreset;
    private Transform _worldHolder;
    private TileManager _tileManager;

    [Inject]
    public void Construct(ITileFactory tileFactory, WorldPreset worldPreset, TileManager tileManager,
        [Inject(Id = "WorldSpawn")] Transform worldHolder)
    {
        _tileFactory = tileFactory;
        _worldPreset = worldPreset;
        _worldHolder = worldHolder;
        _tileManager = tileManager;
    }

    public void Initialize()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        var levelSize = _worldPreset.LevelSize;

        for (int y = 0; y < levelSize.First; y++)
        {
            for (int x = 0; x < levelSize.Second; x++)
            {
                var hasTile = GetTilePrefab(x, y, out var tilePrefab);
                if (!hasTile)
                    continue;

                Vector3 pos = GetSpawnPosition(x, y, _worldPreset.TileSize);
                var tile = _tileFactory.Create(tilePrefab, pos);
                tile.Transform.SetParent(_worldHolder);
                _tileManager.AddTile(tile);
            }
        }
    }

    private Vector3 GetSpawnPosition(int tileX, int tileY, float tileSize)
    {
        var position = new Vector3(tileX * tileSize, 0, tileY * tileSize) -
                       new Vector3(_worldPreset.LevelSize.First / 2, 0f, _worldPreset.LevelSize.Second / 2) * tileSize;

        return position;
    }

    private bool GetTilePrefab(int x, int y, out GameObject tilePrefab)
    {
        tilePrefab = null;
        var map = _worldPreset.Map;
        var mapRows = map.Split("\n");

        if (mapRows[y].Length < x)
        {
            return false;
        }

        var tileData = mapRows[y][x].ToString();
        var parseSuccess = int.TryParse(tileData, out int mapTileData);

        if (!parseSuccess || _worldPreset.TileList.Length < mapTileData)
        {
            return false;
        }

        tilePrefab = _worldPreset.TileList[mapTileData];

        return tilePrefab != null;
    }
}