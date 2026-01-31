using System;
using SurviveProject;
using UnityEngine;
using Zenject;

public class RoadBuildingStrategy : IBuildingStrategy
{
    private readonly TileManager _tileManager;
    private readonly BuildingFactory _buildingFactory;
    private readonly RoadsService _roadsService;
    private readonly BuildingService _buildingService;
    
    private bool _isBuildingSelected = false;
    private bool _isMultiBuildEnabled = false;
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
        
        _tileInputService.OnClickActionHold -= ActionHoldHandler;

    }

    private void ActionHoldHandler(bool toggle)
    {
        _isBuildActionPressed = toggle;

        if (toggle)
        {
           
        }
        else
        {
        }
    }

    public void BuildingTileCheck(WorldTileView view)
    {
        if (!_isBuildingSelected)
        {
            _selectedBuilding = InitBuildingTile(_buildingData);
            return;
        }
       
        if (view == null)
        {
            _selectedBuilding.View.transform.position = new Vector3(0,-100,0);
            return;
        }
        
        var tile = _tileManager.GetTileByView(view);

        if (tile == null)
        {
            return;
        }
        
        if (!tile.IsEmpty)
        {
            return;
        }

        if (tile.HasContent)
        {
            return;
        }

        if (_isBuildActionPressed)
        {
            _selectedBuilding.View.transform.position = view.BuildingHolder.position;
            
            SetPlacementValid(tile.IsEmpty);
        }
    }

    public void BuildingTileClick(WorldTileView obj)
    {
    }
    
    public bool TryCompleteBuilding(WorldTileView worldTileView)
    {
        return false;
    }

    public void CancelBuild()
    {
        if (!_isBuildingSelected)
        {
            return;
        }
        
        _selectedBuilding.Dispose();
        
        _selectedBuilding = null;
        _isBuildingSelected = false;
        _buildingService.ToggleBuildingIndicator(false);
    }

    public void ChangeMultiBuildModifier(bool toggle) {}
    
    
    public void RotateBuilding(float angle) {}

    public void SetPlacementValid(bool valid)
    {
        _selectedBuilding.View.SetIsBuildable(valid);
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