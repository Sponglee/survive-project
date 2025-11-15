using System;
using Zenject;


namespace SurviveProject
{
    public class PlayerInputProvider : IInitializable, IDisposable
    {
        private PlayerInputService _playerInputService;
        private BuildingProvider _buildingProvider;
        
        private bool _isBuildMenuActive = false; 
        
        public PlayerInputProvider(
            PlayerInputService playerInputService,
            BuildingProvider buildingProvider)
        {
            _playerInputService = playerInputService;
            _buildingProvider = buildingProvider;
        }
        
        public void Initialize()
        {
            _playerInputService.OnBuildMenuPressed += BuildMenuToggleHandler;
        }

        public void Dispose()
        {
            _playerInputService.OnBuildMenuPressed += BuildMenuToggleHandler;
        }
        
        private void BuildMenuToggleHandler()
        {
            _isBuildMenuActive = !_isBuildMenuActive;
            _buildingProvider.ToggleBuildMenu(_isBuildMenuActive);
        }
    }
}