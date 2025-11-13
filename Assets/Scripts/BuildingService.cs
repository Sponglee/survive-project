using System;
using SurviveProject;
using UnityEngine;
using Zenject;

public class BuildingService : IDisposable, IInitializable
{
    private readonly TileInputService _tileInputService;
    private readonly PlayerInputService _playerInputService;
    
    private readonly BuildingFactory _buildingFactory;
    
    private Camera _camera;
    private LayerMask _raycastLayerMask;

    private IBuildingStrategy _selectedBuild;
    
    public BuildingService(
        TileInputService tileInputService,
        PlayerInputService playerInputService,
        BuildingFactory buildingFactory)
    {
        _tileInputService = tileInputService;
        _playerInputService = playerInputService;
        _buildingFactory = buildingFactory;
    }

    public void Initialize()
    {
        _tileInputService.OnTileHover += TileHoverHandler;
        _tileInputService.OnTileClicked += TileClickHandler;
        _playerInputService.OnCancelButtonPressed += CancelBuildHandler;
        _playerInputService.OnBuildModifierChanged += MultiBuildModifierChangedHandler;
    }

    public void Dispose()
    {
        _tileInputService.OnTileHover -= TileHoverHandler;
        _tileInputService.OnTileClicked -= TileClickHandler;
        _playerInputService.OnCancelButtonPressed -= CancelBuildHandler;
        _playerInputService.OnBuildModifierChanged -= MultiBuildModifierChangedHandler;

        _selectedBuild?.Dispose();
    }

    public void StartBuild(BuildingData targetBuildingData)
    {
        _selectedBuild = GetBuildStrategy(targetBuildingData);
        
        _selectedBuild?.Initialize(targetBuildingData);
    }
    
    public void CancelBuild()
    {
        if (_selectedBuild == null)
        {
            return;
        }
        
        _selectedBuild.Dispose();
        _selectedBuild = null;
    }
    
    private IBuildingStrategy GetBuildStrategy(BuildingData targetBuildingData)
    {
        var type = targetBuildingData.Type;
        var buildingStrategy = _buildingFactory.CreateStrategy(type);
        return buildingStrategy;
    }


    private void TileHoverHandler(WorldTileView tile)
    {
        _selectedBuild?.TileHoverHandler(tile);
    }
    
    private void TileClickHandler(WorldTileView obj)
    {
        _selectedBuild?.TileClickHandler(obj);
    }
    
    private void CancelBuildHandler()
    {
        _selectedBuild?.DeselectBuilding();
    }
    
    private void MultiBuildModifierChangedHandler(bool toggle)
    {
       _selectedBuild.MultiBuildModifierChangedHandler(toggle);
    }
}

public interface IBuildingStrategy
{
    public void Initialize(BuildingData targetBuildingData);
    public void DeselectBuilding();
    void TileHoverHandler(WorldTileView tile);
    void TileClickHandler(WorldTileView obj);
    void CancelBuildHandler();
    void MultiBuildModifierChangedHandler(bool toggle);
    void Dispose();
}

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

    public void TileHoverHandler(WorldTileView tile)
    {
        if (!_isBuildingSelected)
        {
            return;
        }
        
        _selectedBuilding.View.transform.position = tile.BuildingHolder.position;
    }

    public void TileClickHandler(WorldTileView obj)
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

public class ResourceBuilding : IBuildingStrategy
{
    private readonly TileManager _tileManager;
    private readonly BuildingFactory _buildingFactory;
    
    private bool _isBuildingSelected = false;
    private bool _isMultiBuildEnabled = false;
    private BuildingController _selectedBuilding;
    
    public ResourceBuilding(
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

        if (!tile.HasContent || tile.MapContentType != MapContentType.Resource)
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

        if (!tile.HasContent || tile.MapContentType != MapContentType.Resource)
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

public class RoadBuilding : IBuildingStrategy
{
    public void Initialize(BuildingData targetBuildingData)
    {
        throw new NotImplementedException();
    }

    public void DeselectBuilding()
    {
        throw new NotImplementedException();
    }

    public void TileHoverHandler(WorldTileView tile)
    {
        throw new NotImplementedException();
    }

    public void TileClickHandler(WorldTileView obj)
    {
        throw new NotImplementedException();
    }

    public void CancelBuildHandler()
    {
        throw new NotImplementedException();
    }

    public void MultiBuildModifierChangedHandler(bool toggle)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}