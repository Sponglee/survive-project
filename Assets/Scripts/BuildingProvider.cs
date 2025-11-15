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
        
        public BuildingProvider(
            BuildMenuView view,
            BuildingService buildingService,
            TileInputService tileInputService,
            BuildingsPreset buildingsPreset,
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
            
            _buildingMenuController.Initialize();
        }
        public void Initialize()
        {
            _buildingMenuController.OnBuyItemSelected += BuildingSelectedHandler;
        }
        
        public void Dispose()
        {
            _buildingMenuController.OnBuyItemSelected -= BuildingSelectedHandler;
            _buildingMenuController.Dispose();
        }

        private void BuildingSelectedHandler(BuildingData data)
        {
            _tileInputService.SelectTile(null);
            _buildingService.StartBuild(data);
        }

        public void ToggleBuildMenu(bool isBuilMenuActive)
        {
            _buildingMenuController.ToggleBuildMenu(isBuilMenuActive);
        }
    }
}