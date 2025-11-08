using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class BuildingFactory
    {
        private readonly DiContainer _container;
    
        public BuildingFactory(DiContainer container)
        {
            _container = container; 
        }
  
        public BuildingView Create(GameObject prefab)
        {
            var building = _container.InstantiatePrefabForComponent<BuildingView>(prefab);
            return building;
        }
    }
}