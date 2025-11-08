using System;
using UnityEngine;
using Zenject;


namespace SurviveProject
{
    public class PlayerInputProvider : IInitializable, IDisposable
    {
        private PlayerInputService _playerInputService;
        private BuildMenuView _buildMenuView;
        
        private bool _canSelectTile = true;
        
        public PlayerInputProvider(
            PlayerInputService playerInputService)

        {
            _playerInputService = playerInputService;
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
          Debug.Log("BUY MENU");   
        }
    }
}