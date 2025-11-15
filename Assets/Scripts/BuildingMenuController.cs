using System;
using Zenject;

namespace SurviveProject
{
    public class BuildingMenuController : IDisposable
    {
        public event Action<BuildingData> OnBuyItemSelected;
        
        private BuildMenuModel _model;
        private BuildMenuView _view;
        
        [Inject]
        public BuildingMenuController(BuildMenuModel menuModel, BuildMenuView view)
        {
            _model = menuModel;
            _view = view;
        }
        
        public void Initialize()
        {
            var menuItems = _model.BuildingMenuItems;

            foreach (var menuItem in menuItems)
            {
                menuItem.Initialize();
                menuItem.OnBuildingMenuItemSelected += BuildButtonHandler;
            }
        }

        public void ToggleBuildMenu(bool toggle)
        {
            _view.gameObject.SetActive(toggle);
        }

        public void Dispose()
        {
            var menuItems = _model.BuildingMenuItems;
         
            foreach (var menuItem in menuItems)
            {
                menuItem.OnBuildingMenuItemSelected -= BuildButtonHandler;
            }

            _model = null;
            _view = null;
        }


        private void BuildButtonHandler(BuildingData data)
        {
            OnBuyItemSelected?.Invoke(data);
        }
    }
}