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
    private BuildingData _selectedBuilding;
    
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
        _tileInputService.OnTileClicked += TileClickHandler;
    }

    public void Dispose()
    {
        _tileInputService.OnTileClicked -= TileClickHandler;
        _currentSelectedTileView?.Dispose();
        _clickAction?.Dispose();
    }

    public void SelectBuilding(BuildingData targetBuilding)
    {
        _selectedBuilding = targetBuilding;
        _isBuildingSelected = targetBuilding != null;
    }
    
    private void TileClickHandler(WorldTileView obj)
    {
        if (!_isBuildingSelected || obj == null)
        {
            return;
        }
        Debug.Log($"BUILT {_selectedBuilding}");
        var tile = _tileManager.GetTileByView(obj);

        var data = _selectedBuilding;

        var buildingModel = new BuildingModel(data);
        var buildingView = _buildingFactory.Create(_selectedBuilding.Prefab);
        var buildingController = new BuildingController(buildingView, buildingModel);
        buildingView.transform.SetParent(tile.BuildingHolder);
        buildingView.transform.position = tile.MapContentHolder.position;
        tile.SetBuilding(buildingController);

        _isBuildingSelected = false;
        _selectedBuilding = null;
    }
}