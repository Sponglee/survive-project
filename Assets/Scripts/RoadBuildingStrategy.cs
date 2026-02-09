using System;
using System.Collections.Generic;
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
    
    // Preview only
    private List<BuildingController> _previewRoadControllers = new List<BuildingController>();
    private List<RoadElementView> _previewRoadElements = new List<RoadElementView>();
    
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
        ClearAllPreview();
        _buildingService.ToggleBuildingIndicator(false);
        _tileInputService.OnClickActionHold -= ActionHoldHandler;
    }

    private void ActionHoldHandler(bool isPressed)
    {
        _isBuildActionPressed = isPressed;
        
        if (!isPressed)
        {
            // Finalize on release
            FinalizePreview();
            _lastProcessedTile = null;
        }
    }

    public void BuildingTileCheck(WorldTileView view)
    {
        if (!_isBuildActionPressed || view == null)
        {
            return;
        }
        
        // Skip if same tile
        if (view == _lastProcessedTile)
        {
            return;
        }
        
        // Check if can place
        if (!CanPlaceRoadOn(view))
        {
            return;
        }
        
        // Check adjacency if we have a previous tile
        if (_lastProcessedTile != null)
        {
            if (!AreAdjacent(_lastProcessedTile.TileCoords, view.TileCoords))
            {
                return;
            }
        }
        
        // Create preview road tile
        CreatePreviewRoadTile(view);
        
        // Create preview connection if we have a previous tile
        if (_lastProcessedTile != null)
        {
            CreatePreviewConnection(_lastProcessedTile, view);
        }
        
        _lastProcessedTile = view;
    }

    private bool CanPlaceRoadOn(WorldTileView view)
    {
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

    private void CreatePreviewRoadTile(WorldTileView view)
    {
        var coords = view.TileCoords;
        
        // Skip if road already exists (either finalized or preview)
        if (_roadsService.HasRoadAt(coords))
        {
            return;
        }
        
        // Check if we already created a preview for this tile
        foreach (var controller in _previewRoadControllers)
        {
            if (controller.View.transform.position == view.BuildingHolder.position)
            {
                return;
            }
        }
        
        // Create preview road
        var buildingModel = new BuildingModel(_buildingData);
        var buildingView = _buildingFactory.CreateView(_buildingData.Prefab);
        var roadController = new BuildingController(buildingView, buildingModel);
        
        roadController.View.transform.position = view.BuildingHolder.position;
        roadController.View.SetIsBuildable(true);
        
        _previewRoadControllers.Add(roadController);
    }

    private void CreatePreviewConnection(WorldTileView tileA, WorldTileView tileB)
    {
        var roadTileA = _roadsService.GetRoadTile(tileA.TileCoords);
        var roadTileB = _roadsService.GetRoadTile(tileB.TileCoords);
        
        Transform connectionPointA = null;
        Transform connectionPointB = null;
        
        // Get connection point A
        if (roadTileA != null)
        {
            connectionPointA = roadTileA.GetConnectionPoint();
        }
        else
        {
            // Use preview
            var roadView = GetPreviewRoadView(tileA.BuildingHolder.position);
            connectionPointA = (roadView as RoadView)?.RoadConnectionPoint;
        }
        
        // Get connection point B
        if (roadTileB != null)
        {
            connectionPointB = roadTileB.GetConnectionPoint();
        }
        else
        {
            var roadView = GetPreviewRoadView(tileB.BuildingHolder.position);
            connectionPointB = (roadView as RoadView)?.RoadConnectionPoint;
        }
        
        if (connectionPointA == null || connectionPointB == null)
        {
            return;
        }
        
        // Get prefab
        if (_roadsPreset.RoadsList == null || _roadsPreset.RoadsList.Count == 0)
        {
            return;
        }
        
        var roadElementPrefab = _roadsPreset.RoadsList[0].RoadElementPrefab;
        if (roadElementPrefab == null)
        {
            return;
        }
        
        // Spawn preview connection
        var roadElementObj = GameObject.Instantiate(roadElementPrefab);
        var roadElement = roadElementObj.GetComponent<RoadElementView>();
        
        if (roadElement == null)
        {
            GameObject.Destroy(roadElementObj);
            return;
        }
        
        roadElementObj.transform.SetParent(connectionPointA);
        roadElementObj.transform.localPosition = Vector3.zero;
        roadElementObj.transform.localRotation = Quaternion.identity;
        roadElementObj.transform.localScale = Vector3.one;
        
        roadElement.SetConnectionPoints(connectionPointA.position, connectionPointB.position);
        
        _previewRoadElements.Add(roadElement);
    }

    private BuildingView GetPreviewRoadView(Vector3 position)
    {
        foreach (var controller in _previewRoadControllers)
        {
            if (controller.View.transform.position == position)
            {
                return controller.View;
            }
        }
        return null;
    }

    private void FinalizePreview()
    {
        // Finalize all preview road tiles
        foreach (var controller in _previewRoadControllers)
        {
            var view = _tileManager.GetViewByCoord(GetCoordsFromPosition(controller.View.transform.position));
            if (view == null) continue;
            
            var tile = _tileManager.GetTileByView(view);
            if (tile == null || (!tile.IsEmpty || tile.HasContent)) continue;
            
            // Finalize tile
            controller.View.transform.SetParent(tile.BuildingHolder);
            controller.View.transform.position = tile.BuildingHolder.position;
            tile.SetBuilding(controller);
            tile.SetState(TileState.Occupied);
            
            _roadsService.RegisterRoadTile(controller, tile);
            OnCompletedBuild?.Invoke(view);
        }
        
        // Finalize connections - create them in RoadsService
        // (Preview connections are just visual, now create real ones)
        for (int i = 0; i < _previewRoadControllers.Count - 1; i++)
        {
            var posA = _previewRoadControllers[i].View.transform.position;
            var posB = _previewRoadControllers[i + 1].View.transform.position;
            
            var coordsA = GetCoordsFromPosition(posA);
            var coordsB = GetCoordsFromPosition(posB);
            
            if (_roadsPreset.RoadsList != null && _roadsPreset.RoadsList.Count > 0)
            {
                _roadsService.CreateConnection(coordsA, coordsB, _roadsPreset.RoadsList[0].RoadElementPrefab);
            }
        }
        
        ClearAllPreview();
    }

    private Vector2Int GetCoordsFromPosition(Vector3 position)
    {
        // Find the tile at this position
        foreach (var kvp in _tileManager.ActiveTiles)
        {
            var view = _tileManager.GetViewByCoord(kvp.Key);
            if (view != null && view.BuildingHolder.position == position)
            {
                // Convert Vector2 to Vector2Int
                return new Vector2Int((int)kvp.Key.x, (int)kvp.Key.y);
            }
        }
        return Vector2Int.zero;
    }

    private void ClearAllPreview()
    {
        // Destroy preview road elements
        foreach (var element in _previewRoadElements)
        {
            if (element != null)
            {
                GameObject.Destroy(element.gameObject);
            }
        }
        _previewRoadElements.Clear();
        
        // Don't dispose preview controllers if they're finalized
        // Only clear the list
        _previewRoadControllers.Clear();
    }

    public bool CanPlaceBuilding(WorldTileView view) => CanPlaceRoadOn(view);
    public void BuildingTileClick(WorldTileView obj) { }
    public bool TryCompleteBuilding(WorldTileView obj) => true;
    
    public void CancelBuild()
    {
        _buildingService.ToggleBuildingIndicator(false);
        
        // Dispose preview controllers since we're canceling
        foreach (var controller in _previewRoadControllers)
        {
            controller.Dispose();
        }
        
        ClearAllPreview();
        _isBuildActionPressed = false;
        _lastProcessedTile = null;
    }

    public void ChangeMultiBuildModifier(bool toggle) { }
    public void RotateBuilding(float angle) { }
    public void SetPlacementValid(bool valid) { }
}