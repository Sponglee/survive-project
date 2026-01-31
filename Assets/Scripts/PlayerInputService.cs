using System;
using UnityEngine.InputSystem;
using Zenject;

namespace SurviveProject
{
    public class PlayerInputService : IInitializable, IDisposable
    {
        public event Action<bool> OnBuildModifierChanged;
        public event Action OnBuildMenuPressed;
        public event Action OnCancelButtonPressed;
        
        private InputAction _buildMenuAction;
        private InputAction _cancelButtonAction;
        private InputAction _multiBuildModifierAction;
        private InputAction _actionButtonPressed;
        
        public PlayerInputService(InputActionAsset input)
        {
            var map = input.FindActionMap("UI");
                map.Enable();
            _buildMenuAction = map.FindAction("BuildButton");
            _cancelButtonAction = map.FindAction("CancelButton");
            _multiBuildModifierAction = map.FindAction("MultiBuildModifier");
        }
        public void Initialize()
        {
            _buildMenuAction.performed += BuildMenuHandler;
            _cancelButtonAction.performed += CancelButtonhandler;
            
            _multiBuildModifierAction.started += MultiBuildModifierEnabledHandler;
            _multiBuildModifierAction.canceled += MultiBuildModifierDisabledHandler;
            
            _buildMenuAction.Enable();
            _cancelButtonAction.Enable();
            _multiBuildModifierAction.Enable();
        }
        
        public void Dispose()
        {
            _buildMenuAction.performed -= BuildMenuHandler;
            _cancelButtonAction.performed -= CancelButtonhandler;
            
            _multiBuildModifierAction.started += MultiBuildModifierEnabledHandler;
            _multiBuildModifierAction.canceled += MultiBuildModifierDisabledHandler;

            _buildMenuAction?.Disable();
            _cancelButtonAction.Disable();
            _multiBuildModifierAction.Disable();
        }

        private void CancelButtonhandler(InputAction.CallbackContext obj)
        {
           OnCancelButtonPressed?.Invoke();
        }


        private void BuildMenuHandler(InputAction.CallbackContext obj)
        {
            OnBuildMenuPressed?.Invoke();
        }
        
        private void MultiBuildModifierEnabledHandler(InputAction.CallbackContext obj)
        {
            OnBuildModifierChanged?.Invoke(true);
        }
        
        private void MultiBuildModifierDisabledHandler(InputAction.CallbackContext obj)
        {
            OnBuildModifierChanged?.Invoke(false);
        }
    }
}