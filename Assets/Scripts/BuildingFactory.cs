using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class BuildingFactory
    {
        private readonly DiContainer _container;
        private readonly TileManager _tileManager;
        
        public BuildingFactory(
            DiContainer container,
            TileManager tileManager)
        {
            _container = container;
            _tileManager = tileManager;
        }

        public IBuildingStrategy CreateStrategy(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.SimpleBuilding:
                    return new SimpleBuilding(_tileManager, this);
                case BuildingType.ResourceBuilding:
                    return new ResourceBuilding(_tileManager, this);
                default:
                    return null;
            }
        }
  
        public BuildingView CreateView(GameObject prefab)
        {
            var building = _container.InstantiatePrefabForComponent<BuildingView>(prefab);
            return building;
        }
    }
}