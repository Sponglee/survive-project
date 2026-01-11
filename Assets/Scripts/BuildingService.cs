using System;
using SurviveProject;
using UnityEngine;
using Zenject;

public class BuildingService : IDisposable, IInitializable
{
    private readonly BuildingFactory _buildingFactory;
    
    private Camera _camera;
    private LayerMask _raycastLayerMask;

    private IBuildingStrategy _selectedBuild;
    
    public BuildingService(
        BuildingFactory buildingFactory)
    {
        _buildingFactory = buildingFactory;
    }

    public void Initialize()
    {
      
    }

    public void Dispose()
    {
        _selectedBuild?.Dispose();
    }

    public void StartBuild(BuildingData targetBuildingData)
    {
        _selectedBuild = GetBuildStrategy(targetBuildingData);
        
        _selectedBuild?.Initialize(targetBuildingData);
    }
    
    private IBuildingStrategy GetBuildStrategy(BuildingData targetBuildingData)
    {
        var type = targetBuildingData.Type;
        var buildingStrategy = _buildingFactory.CreateStrategy(type);
        return buildingStrategy;
    }


    public void TileHoverHandler(WorldTileView tile)
    {
        _selectedBuild?.TileHoverHandler(tile);
    }
    
    public void TileClickHandler(WorldTileView obj)
    {
        _selectedBuild?.TileClickHandler(obj);
    }
    
    public void CancelBuildHandler()
    {
        _selectedBuild?.CancelBuildHandler();
    }
    
    public void MultiBuildModifierChangedHandler(bool toggle)
    {
       _selectedBuild.MultiBuildModifierChangedHandler(toggle);
    }
}