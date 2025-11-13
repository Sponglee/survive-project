using System;
using UnityEngine;
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

        public void Dispose()
        {
            _model = null;
            
            _view?.BuildButton.onClick.RemoveListener(BuildButtonHandler);
            _view = null;
        }

        public void Initialize()
        {
            _view.BuildButton.onClick.AddListener(BuildButtonHandler);
        }

        private void BuildButtonHandler()
        {
            OnBuyItemSelected?.Invoke(_model.BuildingData);
        }
    }
}