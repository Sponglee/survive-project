using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class BuildingFactory
    {
        private readonly DiContainer _container;
        private readonly TileManager _tileManager;
        private readonly RoadsService _roadsService;
        private readonly TileInputService _tileInputService;
        private readonly RoadsPreset _roadsPreset;
        
        public BuildingFactory(
            DiContainer container,
            RoadsService roadsService,
            TileInputService tileInputService,
            TileManager tileManager,
            RoadsPreset roadsPreset)
        {
            _container = container;
            _tileManager = tileManager;
            _tileInputService = tileInputService;
            _roadsService = roadsService;
            _roadsPreset = roadsPreset;
        }

        public IBuildingStrategy CreateStrategy(BuildingType type, BuildingService buildingService)
        {
            return type switch
            {
                BuildingType.SimpleBuilding => new SimpleBuildingStrategy(_tileManager, this),
                BuildingType.ResourceBuilding => new ResourceBuildingStrategy(_tileManager, this),
                BuildingType.RoadBuilding => new RoadBuildingStrategy(_tileManager, _roadsService, buildingService, _tileInputService, this, _roadsPreset),
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