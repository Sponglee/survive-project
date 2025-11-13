using System;
using UnityEngine;

namespace SurviveProject
{
    public class WorldTileController : ITile, IDisposable
    {
        private readonly WorldTileModel _model;
        private readonly WorldTileView _view;
        
        private readonly TileManager _tileManager;
        private readonly TileInputService _inputService;

        public MapContentType MapContentType => _model.MapContent.MapContentType;

        public Transform MapContentHolder => _view.ContentHolder;
        public Transform BuildingHolder => _view.BuildingHolder;
        public TileState TileState => _model.TileState;
        public bool IsEmpty => TileState == TileState.Free;
        public bool HasContent => _model.MapContent != null;
        
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