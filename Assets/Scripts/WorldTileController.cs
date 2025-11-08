using System;
using UnityEngine;

namespace SurviveProject
{
    public class WorldTileController : ITile, IDisposable
    {
        private WorldTileModel _model;
        private WorldTileView _view;
        
        private TileManager _tileManager;
        private TileInputService _inputService;

        public Transform Transform => _view.transform;
        public Transform MapContentHolder => _view.ContentHolder;
        public Transform BuildingHolder => _view.BuildingHolder;
        
        public TileState TileState => _model.TileState;
        public MapContentType MapContentType => _model.MapContent.MapContentType;

        public WorldTileController(
            WorldTileModel model, 
            WorldTileView view,
            TileInputService tileInputService)
        {
            _model = model;
            _view = view;
            
            _inputService = tileInputService;
            _inputService.OnTileClicked += TileClickedHandler;
        }

        public void InitializeData()
        {
            
        }
        
        public void Dispose()
        {
            _inputService.OnTileClicked -= TileClickedHandler;

            _view?.Dispose();
        }

        public void SetState(TileState state)
        {
            _model.TileState = state;
        }

        public void SetContent(IMapContent mapContent)
        {
            _model.MapContent = mapContent;
        }
        
        public void SetBuilding(BuildingController building)
        {
            _model.BuildingController = building;
        }

        private void TileClickedHandler(WorldTileView obj)
        {
            _view.IndicatorView.ToggleOutline(obj != null && _view == obj);
        }
    }
}