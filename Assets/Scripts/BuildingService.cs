using System;
using SurviveProject;
using UnityEngine;
using Zenject;

public class BuildingService : IDisposable, IInitializable
{
    private readonly BuildingFactory _buildingFactory;
    private readonly BuildIndicatorView _buildingIndicatorView;

    private Camera _camera;
    private LayerMask _raycastLayerMask;

    private IBuildingStrategy _selectedBuild;
    private bool _multiBuildModifier;
    private bool _isBuildingInProgress;

    public BuildingService(
        BuildingFactory buildingFactory,
        BuildIndicatorView buildingIndicatorView)
    {
        _buildingFactory = buildingFactory;
        _buildingIndicatorView = buildingIndicatorView;
    }

    public bool IsBuildingInProgress => _isBuildingInProgress;
    public event Action<WorldTileView> OnBuildingCompleted;
    
    public void Initialize()
    {
      
    }

    public void Dispose()
    {
       var disposableBuild = _selectedBuild as IDisposable;
           disposableBuild?.Dispose();
    }

    public void ToggleBuildingIndicator(bool toggle)
    {
        _buildingIndicatorView.ToggleIndicator(toggle);
    }

    public void StartBuild(BuildingData targetBuildingData)
    {
        _selectedBuild = GetBuildStrategy(targetBuildingData);

        if (_selectedBuild == null)
        {
            Debug.LogError("No Building found");
            return;
        }

        
        _selectedBuild.Initialize(targetBuildingData);
        _selectedBuild.OnCompletedBuild += SelectedBuildingCompleted;
        _isBuildingInProgress = true;
    }

    public void BuildingAtTileHover(WorldTileView tile)
    {
        _selectedBuild?.BuildingTileCheck(tile);
    }

    public void BuildingTileClick(WorldTileView tile)
    {
        _selectedBuild?.BuildingTileClick(tile);
    }
    
    public void SetMultiBuildModifier(bool toggle)
    {
        _multiBuildModifier = toggle;
       _selectedBuild?.ChangeMultiBuildModifier(_multiBuildModifier);
    }
    
    public bool TryCompleteBuild(WorldTileView obj)
    {
        var isCompleted = _selectedBuild != null && _selectedBuild.TryCompleteBuilding(obj);

        if (!isCompleted)
        {
            return false;
        }

        if (_multiBuildModifier)
        {
           return false;
        }
       
        _selectedBuild.OnCompletedBuild -= SelectedBuildingCompleted;
        _isBuildingInProgress = false;
       return true;

    }
    
    public void CancelBuild()
    {
        _selectedBuild?.CancelBuild();
        
        if (_selectedBuild != null)
        {
            _selectedBuild.OnCompletedBuild -= SelectedBuildingCompleted;
            _isBuildingInProgress = false;

        }
    }

    public void RotateBuilding(float rotationDirection)
    {
        var angle = rotationDirection > 0 ? 90f : -90f;
        _selectedBuild.RotateBuilding(angle);
    }
    
    private IBuildingStrategy GetBuildStrategy(BuildingData targetBuildingData)
    {
        var type = targetBuildingData.Type;
        var buildingStrategy = _buildingFactory.CreateStrategy(type, this);
        return buildingStrategy;
    }
    
    private void SelectedBuildingCompleted(WorldTileView obj)
    {
        OnBuildingCompleted?.Invoke(obj);
    }
}
