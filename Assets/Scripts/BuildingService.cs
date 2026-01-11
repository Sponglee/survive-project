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
       var disposableBuild = _selectedBuild as IDisposable;
           disposableBuild?.Dispose();
    }

    public void StartBuild(BuildingData targetBuildingData)
    {
        _selectedBuild = GetBuildStrategy(targetBuildingData);
        
        _selectedBuild?.Initialize(targetBuildingData);
    }

    public void BuildingAtTileHover(WorldTileView tile)
    {
        _selectedBuild?.BuildingTileCheck(tile);
    }
    
    public void SetMultiBuildModifier(bool toggle)
    {
       _selectedBuild?.ChangeMultiBuildModifier(toggle);
    }
    
    public bool TryCompleteBuild(WorldTileView obj)
    {
        return _selectedBuild != null && _selectedBuild.TryCompleteBuilding(obj);
    }
    
    public void CancelBuild()
    {
        _selectedBuild?.CancelBuild();
    }
    
    public void RotateBuilding(float rotationDirection)
    {
        var angle = rotationDirection > 0 ? 90f : -90f;
        _selectedBuild.RotateBuilding(angle);
    }
    
    private IBuildingStrategy GetBuildStrategy(BuildingData targetBuildingData)
    {
        var type = targetBuildingData.Type;
        var buildingStrategy = _buildingFactory.CreateStrategy(type);
        return buildingStrategy;
    }
}
