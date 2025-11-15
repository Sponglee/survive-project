using SurviveProject;
using UnityEngine;

public class SimpleBuilding : IBuildingStrategy
{
    private readonly TileManager _tileManager;
    private readonly BuildingFactory _buildingFactory;
    
    private bool _isBuildingSelected = false;
    private bool _isMultiBuildEnabled = false;
    private BuildingController _selectedBuilding;
    
    public SimpleBuilding(
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
    
    public void DeselectBuilding()
    {
        if (!_isBuildingSelected)
        {
            return;
        }

        _selectedBuilding.Dispose();
        
        _selectedBuilding = null;
        _isBuildingSelected = false;
    }

    public void TileHoverHandler(WorldTileView view)
    {
        if (!_isBuildingSelected)
        {
            return;
        }
        
        var tile = _tileManager.GetTileByView(view);

        if (tile == null)
        {
            return;
        }

        if (tile.HasContent)
        {
            return;
        }

        
        _selectedBuilding.View.transform.position = view.BuildingHolder.position;
    }

    public void TileClickHandler(WorldTileView obj)
    {
        if (!_isBuildingSelected || obj == null)
        {
            return;
        }
                
        var tile = _tileManager.GetTileByView(obj);

        if (tile == null)
        {
            return;
        }

        if (tile.HasContent)
        {
            return;
        }
        
        _selectedBuilding.View.transform.SetParent(tile.BuildingHolder);
        _selectedBuilding.View.transform.position = tile.BuildingHolder.position;
        tile.SetBuilding(_selectedBuilding);
        tile.SetState(TileState.Occupied);

        if (_isMultiBuildEnabled)
        {
            Initialize(_selectedBuilding.Data);
            return;
        }

        _selectedBuilding = null;
        _isBuildingSelected = false;
    }

    public void CancelBuildHandler()
    {
        if (_isBuildingSelected)
        {
            DeselectBuilding();
        }
    }

    public void MultiBuildModifierChangedHandler(bool toggle)
    {
        _isMultiBuildEnabled = toggle;
    }
}