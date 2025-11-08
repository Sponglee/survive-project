using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class MapContentItemFactory
    {
        private readonly DiContainer _container;
    
        public MapContentItemFactory(DiContainer container)
        {
            _container = container; 
        }
  
        public MapContentView Create(GameObject prefab)
        {
            var mapContent = _container.InstantiatePrefabForComponent<MapContentView>(prefab);
            return mapContent;
        }
    }
}