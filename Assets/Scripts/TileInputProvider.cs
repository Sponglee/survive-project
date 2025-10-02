using System;
using Zenject;


namespace DefaultNamespace
{
    public class TileInputProvider : IInitializable, IDisposable
    {
        private TileInputService _tileInputService;
        private CameraManager _cameraManager;
        
        public TileInputProvider(
            TileInputService tileInputService,
            CameraManager cameraManager)

        {
            _tileInputService = tileInputService;
            _cameraManager = cameraManager;
        }
        
        public void Initialize()
        {
            _tileInputService.OnTileClicked += TileClickedHandler;
        }

        public void Dispose()
        {
            _tileInputService.OnTileClicked -= TileClickedHandler;
        }
        
        private void TileClickedHandler(WorldTile obj)
        {
            if(obj == null) return;
            var objectTransform = obj.transform;
            _cameraManager.LookAt(objectTransform);
        }
    }
}