using System;
using System.Linq;
using Zenject;

namespace SurviveProject
{
    public class BuildingProvider : IDisposable, IInitializable
    {
        private readonly TileInputService _tileInputService;
        private readonly BuildingMenuController _buildingMenuController;
        private readonly BuildingService _buildingService;
        private readonly CameraInputService _cameraInputService;
        private readonly PlayerInputService _playerInputService;
        
        public BuildingProvider(
            BuildMenuView view,
            BuildingService buildingService,
            TileInputService tileInputService,
            BuildingsPreset buildingsPreset,
            CameraInputService cameraInputService,
            PlayerInputService playerInputService,
            BuildingMenuItemFactory menuItemFactory
            )
        {
            
            var presetList = buildingsPreset.BuildingsList;
            
            var menuItemsList = (from data in presetList select new BuildingMenuItemModel(data) into menuItemModel let menuItemView = menuItemFactory.CreateItemView() select new BuildingMenuItemController(menuItemModel, menuItemView)).ToList();

            var model = new BuildMenuModel(menuItemsList);
            var controller = new BuildingMenuController(model, view);

            _buildingMenuController = controller;
            _buildingService = buildingService;
            _tileInputService = tileInputService;
            _cameraInputService = cameraInputService;
            _playerInputService = playerInputService;
            _buildingMenuController.Initialize();
        }
        public void Initialize()
        {
            _buildingMenuController.OnBuyItemSelected += BuildingSelectedHandler;
            _playerInputService.OnCancelButtonPressed += BuildingCancelledHandler;
            
            _buildingService.OnBuildingCompleted += BuildingCompletedHandler;
            
            _tileInputService.OnTileHover += TileHoverHandler;
            _tileInputService.OnTileClicked += TileClickHandler;
            _playerInputService.OnBuildModifierChanged += MultiBuildModifierChangedHandler;

        }
        
        public void Dispose()
        {
            _buildingMenuController.OnBuyItemSelected -= BuildingSelectedHandler;
            _playerInputService.OnCancelButtonPressed -= BuildingCancelledHandler;
            
            _buildingService.OnBuildingCompleted -= BuildingCompletedHandler;

            ToggleCameraZoomLock(false);
            
            _buildingMenuController.Dispose();
            
            _tileInputService.OnTileHover -= TileHoverHandler;
            _tileInputService.OnTileClicked -= TileClickHandler;
            _playerInputService.OnBuildModifierChanged -= MultiBuildModifierChangedHandler;
            
            _buildingService.Dispose();
        }

        public void ToggleBuildMenu(bool isBuilMenuActive)
        {
            _buildingMenuController.ToggleBuildMenu(isBuilMenuActive);
        }
        
        private void TileHoverHandler(WorldTileView tile)
        {
            _buildingService.BuildingAtTileHover(tile);
        }
    
        private void TileClickHandler(WorldTileView obj)
        {
           _buildingService.BuildingTileClick(obj);
        }
    
        private void MultiBuildModifierChangedHandler(bool toggle)
        {
            _buildingService.SetMultiBuildModifier(toggle);
        }
        
        private void BuildingSelectedHandler(BuildingData data)
        {
            if (_buildingService.IsBuildingInProgress)
            {
                _buildingService.CancelBuild();
                BuildingCancelledHandler();
            }

            
            _tileInputService.SelectTile(null);
            _buildingService.StartBuild(data);
            ToggleCameraZoomLock(true);
        }

        private void BuildingCompletedHandler(WorldTileView obj)
        {
           var isCompleted = _buildingService.TryCompleteBuild(obj);
           if (isCompleted)
           {
                ToggleCameraZoomLock(false);
           } 
        }
        
        private void BuildingCancelledHandler()
        {
            _buildingService.CancelBuild();
            ToggleCameraZoomLock(false);
        }

        private void RotateBuildingHandler(float axisValue)
        {
           _buildingService.RotateBuilding(axisValue);
        }

        private void ToggleCameraZoomLock(bool toggle)
        {
            _cameraInputService.SetIsZoomLocked(toggle);

            if (toggle)
            {
                _cameraInputService.OnCameraZoom += RotateBuildingHandler;
            }
            else
            {
                _cameraInputService.OnCameraZoom -= RotateBuildingHandler;
            }
        }
    }
}