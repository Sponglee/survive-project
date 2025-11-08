using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class WorldTileFactory : ITileFactory
    {
        private readonly DiContainer _container;
        private TileManager _tileManager;
        
        public WorldTileFactory(
            DiContainer container)
        {
            _container = container;
        }
  
        public WorldTileView Create(GameObject prefab)
        {
            var tileView = _container.InstantiatePrefabForComponent<WorldTileView>(prefab);
            return tileView;
        }
    }

}