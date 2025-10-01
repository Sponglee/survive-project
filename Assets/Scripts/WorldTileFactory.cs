using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class WorldTileFactory : ITileFactory
    {
        private readonly DiContainer _container;
    
        public WorldTileFactory(DiContainer container)
        {
            _container = container;
        }
  
        public ITile Create(GameObject prefab, Vector3 position)
        {
            var tile = _container.InstantiatePrefabForComponent<WorldTile>(prefab);
            tile.transform.position = position;
            return tile;
        }
    }

}