using System;
using UnityEngine.InputSystem;
using Zenject;

namespace SurviveProject
{
    public class PlayerInputService : IInitializable, IDisposable
    {
        public event Action OnBuildMenuPressed;
        
        private InputAction _buildMenuAction;
        
        
        public PlayerInputService(InputActionAsset input)
        {
            var map = input.FindActionMap("UI");
                map.Enable();
            _buildMenuAction = map.FindAction("BuildButton");
        }
        
        public void Dispose()
        {
            _buildMenuAction.performed -= BuildMenuHandler;

            _buildMenuAction?.Disable();
        }
        
        public void Initialize()
        {
            _buildMenuAction.performed += BuildMenuHandler;
            
            _buildMenuAction.Enable();
        }

        private void BuildMenuHandler(InputAction.CallbackContext obj)
        {
            OnBuildMenuPressed?.Invoke();
        }
    }
}