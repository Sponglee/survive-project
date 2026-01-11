using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class BuildingFactory
    {
        private readonly DiContainer _container;
        private readonly TileManager _tileManager;
        private readonly RoadsService _roadsService;

        public BuildingFactory(
            DiContainer container,
            RoadsService roadsService,
            TileManager tileManager)
        {
            _container = container;
            _tileManager = tileManager;
            _roadsService = roadsService;
        }

        public IBuildingStrategy CreateStrategy(BuildingType type)
        {
            return type switch
            {
                BuildingType.SimpleBuilding => new SimpleBuildingStrategy(_tileManager, this),
                BuildingType.ResourceBuilding => new ResourceBuildingStrategy(_tileManager, this),
                BuildingType.RoadBuilding => new RoadBuildingStrategy(_tileManager, _roadsService, this),
                _ => null
            };
        }
  
        public BuildingView CreateView(GameObject prefab)
        {
            var building = _container.InstantiatePrefabForComponent<BuildingView>(prefab);
            return building;
        }
    }
}