using System;
using SurviveProject;
using UnityEngine;

public class ResourceBuildingStrategy : IBuildingStrategy, IDisposable
{
    private readonly TileManager _tileManager;
    private readonly BuildingFactory _buildingFactory;
    
    private bool _isBuildingSelected = false;
    private bool _isMultiBuildEnabled = false;
    private BuildingController _selectedBuilding;
    
    public ResourceBuildingStrategy(
        TileManager tileManager,
        BuildingFactory buildingFactory)
    {
        _tileManager = tileManager;
        _buildingFactory = buildingFactory;
    }
    
    public void Initialize(BuildingData targetBuildingData)
    {
        _isBuildingSelected = targetBuildingData != null;
        
        if (targetBuildingData == null)
        {
            return;
        }
        

        var buildingModel = new BuildingModel(targetBuildingData);
        var buildingView = _buildingFactory.CreateView(targetBuildingData.Prefab);
        _selectedBuilding = new BuildingController(buildingView, buildingModel);
        _selectedBuilding.View.transform.position = new Vector3(0,-100,0);
    }
    
    public void Dispose()
    {
        _selectedBuilding?.Dispose();
    }

    public void BuildingTileCheck(WorldTileView view)
    {
        if (!_isBuildingSelected)
        {
            return;
        }

        var tile = _tileManager.GetTileByView(view);
        
        _selectedBuilding.View.transform.position = view.BuildingHolder.position;
        SetPlacementValid(false);

        if (tile == null)
        {
            return;
        }

        if (!tile.HasContent || tile.MapContentType != MapContentType.Resource)
        {
            return;
        }

        SetPlacementValid(true);
    }

    public bool TryCompleteBuilding(WorldTileView obj)
    {
        if (!_isBuildingSelected || obj == null)
        {
            return false;
        }
                
        var tile = _tileManager.GetTileByView(obj);
        
        if (tile == null)
        {
            return false;
        }

        if (!tile.HasContent || tile.MapContentType != MapContentType.Resource)
        {
            return false;
        }
        
        _selectedBuilding.View.transform.SetParent(tile.BuildingHolder);
        _selectedBuilding.View.transform.position = tile.BuildingHolder.position;
        tile.SetBuilding(_selectedBuilding);
        tile.SetState(TileState.Occupied);

        if (_isMultiBuildEnabled)
        {
            Initialize(_selectedBuilding.Data);
            return false;
        }

        _selectedBuilding = null;
        _isBuildingSelected = false;
        
        return true;
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
    }

    public void ChangeMultiBuildModifier(bool toggle)
    {
        _isMultiBuildEnabled = toggle;
    }

    public void RotateBuilding(float angle)
    {
        _selectedBuilding.Rotate(angle);
    }
    
    public void SetPlacementValid(bool valid)
    {
        _selectedBuilding.View.SetIsBuildable(valid);
    }
    
    public bool CheckTilePlacementRule(WorldTileView view)
    {
        var tile = _tileManager.GetTileByView(view);

        if (tile == null)
        {
            return false;
        }

        if (!tile.HasContent || tile.MapContentType != MapContentType.Resource)
        {
            return false;
        }

        return true;
    }
}