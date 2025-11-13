using System;
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
            BuildingsPreset buildingsPreset
            )
        {
            var preset = buildingsPreset.BuildingsList[0];
            var model = new BuildMenuModel(preset);
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
            _buildingService.SelectBuilding(data);
        }
    }

}