using System;
using System.Collections.Generic;
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
            var menuItemsList = new List<BuildingMenuItemController>();
    
            foreach (var data in presetList)
            {
                var menuItemModel = new BuildingMenuItemModel(data);
                var menuItemView = menuItemFactory.CreateItemView();
                var menuItemController = new BuildingMenuItemController(menuItemModel, menuItemView);

                menuItemsList.Add(menuItemController);
            }
            
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
            
            _tileInputService.OnTileHover += _buildingService.TileHoverHandler;
            _tileInputService.OnTileClicked += _buildingService.TileClickHandler;
            _playerInputService.OnBuildModifierChanged += _buildingService.MultiBuildModifierChangedHandler;
        }
        
        public void Dispose()
        {
            _buildingMenuController.OnBuyItemSelected -= BuildingSelectedHandler;
            _playerInputService.OnCancelButtonPressed -= BuildingCancelledHandler;
            _buildingMenuController.Dispose();
            
            _tileInputService.OnTileHover -= _buildingService.TileHoverHandler;
            _tileInputService.OnTileClicked -= _buildingService.TileClickHandler;
            _playerInputService.OnBuildModifierChanged -= _buildingService.MultiBuildModifierChangedHandler;
            
            _buildingService.Dispose();
        }

        private void BuildingSelectedHandler(BuildingData data)
        {
            _tileInputService.SelectTile(null);
            _buildingService.StartBuild(data);
            _cameraInputService.SetIsZoomLocked(true);
        }

        private void BuildingCancelledHandler()
        {
            _buildingService.CancelBuildHandler();
            _cameraInputService.SetIsZoomLocked(false);
        }

        public void ToggleBuildMenu(bool isBuilMenuActive)
        {
            _buildingMenuController.ToggleBuildMenu(isBuilMenuActive);
        }
    }
}