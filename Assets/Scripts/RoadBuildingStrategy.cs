using System;
using SurviveProject;
using UnityEngine;

public class RoadBuildingStrategy : IBuildingStrategy, IDisposable
{
    private readonly TileManager _tileManager;
    private readonly BuildingFactory _buildingFactory;
    private readonly RoadsService _roadsService;
    private readonly BuildingService _buildingService;
    
    private bool _isBuildingSelected = false;
    private bool _isMultiBuildEnabled = true;
    private bool _isBuildActionPressed = false;
    
    private BuildingController _selectedBuilding;
    private BuildingData _buildingData;
    
    private TileInputService _tileInputService;
    
    public RoadBuildingStrategy(
        TileManager tileManager,
        RoadsService roadsService,
        BuildingService buildingService,
        TileInputService tileInputService,
        BuildingFactory buildingFactory)
    {
        _tileManager = tileManager;
        _buildingFactory = buildingFactory;
        _tileInputService = tileInputService;
        _buildingService = buildingService;
        _roadsService = roadsService;
    }
    
    public event Action<WorldTileView> OnCompletedBuild;
    public bool MultibuildModifier => _isMultiBuildEnabled;

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
        _selectedBuilding?.Dispose();
        _buildingService.ToggleBuildingIndicator(false);
        _tileInputService.OnClickActionHold -= ActionHoldHandler;

    }

    private void ActionHoldHandler(bool toggle)
    {
        _isBuildActionPressed = toggle;
        SetPlacementValid(false);

        if (toggle)
        {
            return;
        }
        
        var lastKnownTile = _tileInputService.LastRayCastTile;
        
        var canBuild = CanPlaceBuilding(lastKnownTile);

        if (!canBuild)
        {
            return;
        }
        
        _selectedBuilding.View.transform.position = lastKnownTile.BuildingHolder.position;
            
        OnCompletedBuild?.Invoke(lastKnownTile);
    }

    public void BuildingTileCheck(WorldTileView view)
    {
        var canBuild = CanPlaceBuilding(view);
        if (!canBuild)
        {
            return;
        }

        SetPlacementValid(canBuild);
        OnCompletedBuild?.Invoke(view);
    }

    public bool CanPlaceBuilding(WorldTileView view)
    {
        if (!_isBuildingSelected)
        {
            _selectedBuilding = InitBuildingTile(_buildingData);
        }
       
        if (view == null)
        {
            _selectedBuilding.View.transform.position = new Vector3(0,-100,0);
            return false;
        }
        
        var tile = _tileManager.GetTileByView(view);

        SetPlacementValid(true);
        _selectedBuilding.View.transform.position = view.BuildingHolder.position;
      
        return tile != null && tile.IsEmpty && !tile.HasContent && _isBuildActionPressed;
    }

    public void BuildingTileClick(WorldTileView obj) { }
    
    public bool TryCompleteBuilding(WorldTileView obj)
    {
        if (obj == null)
        {
            _selectedBuilding = null;
            _isBuildingSelected = false;
            return true;
        }
        
        var tile = _tileManager.GetTileByView(obj);

        if (_selectedBuilding == null)
        {
            return true;
        }
        
        _selectedBuilding.View.transform.SetParent(tile.BuildingHolder);
        _selectedBuilding.View.transform.position = tile.BuildingHolder.position;
        tile.SetBuilding(_selectedBuilding);
        tile.SetState(TileState.Occupied);
        _roadsService.RegisterRoad(_selectedBuilding, tile);

        _selectedBuilding = null;
        _isBuildingSelected = false;
        return true;

    }

    public void CancelBuild()
    {
        _buildingService.ToggleBuildingIndicator(false);
        
        if (!_isBuildingSelected)
        {
            return;
        }
        
        _selectedBuilding.Dispose();
        _selectedBuilding = null;
        _isBuildingSelected = false;
    }

    public void ChangeMultiBuildModifier(bool toggle) {}
    
    
    public void RotateBuilding(float angle) {}

    public void SetPlacementValid(bool valid)
    {
        _selectedBuilding?.View.SetIsBuildable(valid);
    }

    private BuildingController InitBuildingTile(BuildingData targetBuildingData)
    {
        var buildingModel = new BuildingModel(targetBuildingData);
        var buildingView = _buildingFactory.CreateView(targetBuildingData.Prefab);
        var selectedBuilding = new BuildingController(buildingView, buildingModel);
        selectedBuilding.View.transform.position = new Vector3(0,-100,0);
        _isBuildingSelected = _buildingData != null;
        return selectedBuilding;
    }
}