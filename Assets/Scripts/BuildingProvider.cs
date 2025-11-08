using System;
using Zenject;


namespace SurviveProject
{
    public class BuildingProvider : IDisposable, IInitializable
    {
        private BuildingMenuController _buildingMenuController;
        private BuildingService _buildingService;
        
        public BuildingProvider(
            BuildMenuView view,
            BuildingService buildingService,
            BuildingsPreset buildingsPreset
            )
        {
            var preset = buildingsPreset.BuildingsList[0];
            var model = new BuildMenuModel(preset);
            var controller = new BuildingMenuController(model, view);

            _buildingMenuController = controller;
            _buildingService = buildingService;
            _buildingMenuController.Initialize();
        }
        
        public void Dispose()
        {
            _buildingMenuController.Dispose();
        }

        public void Initialize()
        {
            _buildingMenuController.OnBuyItemConfirmed += BuildHandler;
        }

        private void BuildHandler(BuildingData data)
        {
            _buildingService.SelectBuilding(data);
        }
    }

}