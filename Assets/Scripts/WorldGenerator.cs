using SurviveProject;
using UnityEngine;
using Zenject;

public class WorldGenerator : IInitializable
{
    private MapContentItemFactory _mapContentItemFactory;
    private WorldPreset _worldPreset;
    private MapContentPreset _mapContentPreset;
    private Transform _worldHolder;
    
    private ITileFactory _tileFactory;
    private TileManager _tileManager;
    private TileInputService _tileInputService;

    [Inject]
    public void Construct(
        ITileFactory tileFactory,
        MapContentItemFactory mapContentItemFactory,
        WorldPreset worldPreset, 
        MapContentPreset mapContentPreset,
        TileManager tileManager,
        TileInputService tileInputService,
        [Inject(Id = "WorldSpawn")] Transform worldHolder)
    {
        _tileFactory = tileFactory;
        _mapContentItemFactory = mapContentItemFactory;
        _worldPreset = worldPreset;
        _mapContentPreset = mapContentPreset;
        _worldHolder = worldHolder;
        _tileManager = tileManager;
        _tileInputService = tileInputService;
    }

    public void Initialize()
    {
        GenerateLevel();
        GenerateMapContent();
    }

    private void GenerateMapContent()
    {
        var tiles = _tileManager.ActiveTiles;
    
        for (var i = 0; i < tiles.Count; i++)
        {
            var tile = tiles[i];
            var hasObject = Random.Range(0, 100) >= 90;
            if (!hasObject)
            {
                continue;
            }

            var prefabFound = GetObjectPrefab(out var mapContentPresetData);
            
            var mapContentView = _mapContentItemFactory.Create(mapContentPresetData.Prefab);
            mapContentView.transform.SetParent(tile.MapContentHolder);
            mapContentView.transform.position = tile.MapContentHolder.position;
            
            var mapContentModel = new MapContentModel(mapContentPresetData.Type);
            var mapContent = new MapContentController(mapContentView, mapContentModel);
            
            tile.SetContent(mapContent);
        }
    }

    private void GenerateLevel()
    {
        var tileId = 0;
        
        var levelSize = _worldPreset.LevelSize;

        for (var y = 0; y < levelSize.First; y++)
        {
            for (var x = 0; x < levelSize.Second; x++)
            {
                var hasTile = GetTilePrefab(x, y, out var tilePrefab);
                if (!hasTile)
                    continue;

                var pos = GetSpawnPosition(x, y, _worldPreset.TileSize);
                var tileView = _tileFactory.Create(tilePrefab);
                tileView.transform.position = pos;
                tileView.transform.SetParent(_worldHolder);
               
                tileView.SetId(tileId);
                var tileModel = new WorldTileModel(tileId, new Vector2(x,y));
                var tile = new WorldTileController(tileModel, tileView, _tileInputService);
                
                _tileManager.AddTile(tileId, tile);
                tileId++;
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
    
    //TODO Generate or get from save
    private bool GetObjectPrefab(out MapContentPresetData mapContentPresetPrefab)
    {
        var objectList = _mapContentPreset.MapContentList;
        var randomIndex = Random.Range(0, objectList.Count);
        var randomObject = objectList[randomIndex];
        mapContentPresetPrefab = randomObject;

        return true;
    }
    
    //TODO get from save
    private bool GetBuildingPrefab(out BuildingData buildingData)
    {
        buildingData = null;
        return false;
    }
}