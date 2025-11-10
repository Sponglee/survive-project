using System;
using SurviveProject;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class BuildingService : IDisposable, IInitializable
{
    private WorldTileView _currentSelectedTileView = null;
    private InputAction _clickAction;

    private TileInputService _tileInputService;
    private TileManager _tileManager;
    private BuildingFactory _buildingFactory;
    
    private Camera _camera;
    private LayerMask _raycastLayerMask;

    private bool _isBuildingSelected = false;
    private BuildingController _selectedBuilding;

    
    public BuildingService(
        TileInputService tileInputService,
        TileManager tileManager,
        BuildingFactory buildingFactory)
    {
        _tileInputService = tileInputService;
        _tileManager = tileManager;
        _buildingFactory = buildingFactory;
    }

    public void Initialize()
    {
        _tileInputService.OnTileHover += TileHoverHandler;
        _tileInputService.OnTileClicked += TileClickHandler;
    }

    public void Dispose()
    {
        _tileInputService.OnTileHover -= TileHoverHandler;
        _tileInputService.OnTileClicked -= TileClickHandler;
        _currentSelectedTileView?.Dispose();
        _clickAction?.Dispose();
    }

    public void SelectBuilding(BuildingData targetBuildingData)
    {
        _isBuildingSelected = targetBuildingData != null;
        
        if (targetBuildingData == null)
        {
            return;
        }
        

        var buildingModel = new BuildingModel(targetBuildingData);
        var buildingView = _buildingFactory.Create(targetBuildingData.Prefab);
        _selectedBuilding = new BuildingController(buildingView, buildingModel);
        _selectedBuilding.View.transform.position = new Vector3(0,-100,0);
    }

    private void TileHoverHandler(WorldTileView tile)
    {
        if (!_isBuildingSelected)
        {
            return;
        }
        
        _selectedBuilding.View.transform.position = tile.BuildingHolder.position;
    }
    
    private void TileClickHandler(WorldTileView obj)
    {
        if (!_isBuildingSelected || obj == null)
        {
            return;
        }
                
        var tile = _tileManager.GetTileByView(obj);
        _selectedBuilding.View.transform.SetParent(tile.BuildingHolder);
        _selectedBuilding.View.transform.position = tile.BuildingHolder.position;
        tile.SetBuilding(_selectedBuilding);
        tile.SetState(TileState.Occupied);
        
        _isBuildingSelected = false;
        _selectedBuilding = null;
    }
}