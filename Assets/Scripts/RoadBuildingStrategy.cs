using System;
using SurviveProject;
using UnityEngine;

public class RoadBuildingStrategy : IBuildingStrategy, IDisposable
{
    private readonly TileManager _tileManager;
    private readonly BuildingFactory _buildingFactory;
    private readonly RoadsService _roadsService;
    private readonly BuildingService _buildingService;
    private readonly RoadsPreset _roadsPreset;
    private readonly TileInputService _tileInputService;
    
    private BuildingData _buildingData;
    private bool _isBuildActionPressed = false;
    private WorldTileView _lastProcessedTile;
    
    public RoadBuildingStrategy(
        TileManager tileManager,
        RoadsService roadsService,
        BuildingService buildingService,
        TileInputService tileInputService,
        BuildingFactory buildingFactory,
        RoadsPreset roadsPreset)
    {
        _tileManager = tileManager;
        _buildingFactory = buildingFactory;
        _tileInputService = tileInputService;
        _buildingService = buildingService;
        _roadsService = roadsService;
        _roadsPreset = roadsPreset;
    }
    
    public event Action<WorldTileView> OnCompletedBuild;
    public bool MultibuildModifier => true;

    public void Initialize(BuildingData targetBuildingData)
    {
        if (targetBuildingData == null)
        {
            return;
        }
        
        _buildingService.ToggleBuildingIndicator(true);
        _tileInputService.OnClickActionHold += ActionHoldHandler;
        _buildingData = targetBuildingData;
    }
    
    public void Dispose()
    {
        _buildingService.ToggleBuildingIndicator(false);
        _tileInputService.OnClickActionHold -= ActionHoldHandler;
    }

    private void ActionHoldHandler(bool isPressed)
    {
        _isBuildActionPressed = isPressed;
        
        if (!isPressed)
        {
            _lastProcessedTile = null;
            return;
        }

        BuildingTileCheck(_tileInputService.LastRayCastTile);
    }

    public void BuildingTileCheck(WorldTileView view)
    {
        if (!_isBuildActionPressed || view == null)
        {
            return;
        }

        TryBuildRoadAt(view);
    }

    private void TryBuildRoadAt(WorldTileView view)
    {
        if (view == _lastProcessedTile)
        {
            return;
        }

        if (_lastProcessedTile != null && !AreAdjacent(_lastProcessedTile.TileCoords, view.TileCoords))
        {
            return;
        }

        if (!CanPlaceRoadOn(view))
        {
            return;
        }

        var roadCreated = EnsureRoadTile(view);
        if (!roadCreated && !_roadsService.HasRoadAt(view.TileCoords))
        {
            return;
        }

        if (_lastProcessedTile != null)
        {
            CreateRoadConnection(_lastProcessedTile.TileCoords, view.TileCoords);
        }

        _lastProcessedTile = view;
    }

    private bool CanPlaceRoadOn(WorldTileView view)
    {
        if (view == null)
        {
            return false;
        }

        var tile = _tileManager.GetTileByView(view);
        if (tile == null)
        {
            return false;
        }
        
        // Can place on empty tiles or existing roads
        return (tile.IsEmpty && !tile.HasContent) || _roadsService.HasRoadAt(view.TileCoords);
    }

    private bool AreAdjacent(Vector2Int coordsA, Vector2Int coordsB)
    {
        int dx = Mathf.Abs(coordsA.x - coordsB.x);
        int dy = Mathf.Abs(coordsA.y - coordsB.y);
        return (dx <= 1 && dy <= 1) && !(dx == 0 && dy == 0);
    }

    private bool EnsureRoadTile(WorldTileView view)
    {
        var coords = view.TileCoords;

        if (_roadsService.HasRoadAt(coords))
        {
            return false;
        }

        var tile = _tileManager.GetTileByView(view);
        if (tile == null || !tile.IsEmpty || tile.HasContent)
        {
            return false;
        }

        var buildingModel = new BuildingModel(_buildingData);
        var buildingView = _buildingFactory.CreateView(_buildingData.Prefab);
        var roadController = new BuildingController(buildingView, buildingModel);

        roadController.View.transform.SetParent(tile.BuildingHolder);
        roadController.View.transform.position = tile.BuildingHolder.position;
        roadController.View.SetIsBuildable(true);

        tile.SetBuilding(roadController);
        tile.SetState(TileState.Occupied);

        _roadsService.RegisterRoadTile(roadController, tile);
        OnCompletedBuild?.Invoke(view);
        return true;
    }

    private void CreateRoadConnection(Vector2Int coordsA, Vector2Int coordsB)
    {
        if (_roadsPreset.RoadsList == null || _roadsPreset.RoadsList.Count == 0)
        {
            return;
        }

        var roadElementPrefab = _roadsPreset.RoadsList[0].RoadElementPrefab;
        if (roadElementPrefab == null)
        {
            return;
        }

        _roadsService.CreateConnection(coordsA, coordsB, roadElementPrefab);
    }

    public bool CanPlaceBuilding(WorldTileView view) => CanPlaceRoadOn(view);
    public void BuildingTileClick(WorldTileView obj) { }
    public bool TryCompleteBuilding(WorldTileView obj) => true;
    
    public void CancelBuild()
    {
        _buildingService.ToggleBuildingIndicator(false);
        _isBuildActionPressed = false;
        _lastProcessedTile = null;
    }

    public void ChangeMultiBuildModifier(bool toggle) { }
    public void RotateBuilding(float angle) { }
    public void SetPlacementValid(bool valid) { }
}
