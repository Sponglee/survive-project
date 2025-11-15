using System;

namespace SurviveProject
{
    public class BuildingMenuItemController : IDisposable
    {
        public event Action<BuildingData> OnBuildingMenuItemSelected;
        
        private BuildingMenuItemView _view;
        private BuildingMenuItemModel _model;
        
        public BuildingMenuItemController(
            BuildingMenuItemModel model, 
            BuildingMenuItemView view)
        {
            _model = model;
            _view = view;
        }
        
        public void Initialize()
        {
            _view.SetData(_model.BuildingData);

            _view.BuyButton.onClick.AddListener(BuildButtonPressedHandler);
        }

        public void Dispose()
        {
            _view.BuyButton.onClick.RemoveListener(BuildButtonPressedHandler);
        }

        private void BuildButtonPressedHandler()
        {
            OnBuildingMenuItemSelected?.Invoke(_model.BuildingData);
        }
    }
}