using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class RoadsService : IInitializable
    {
        private readonly TileManager _tileManager;

        private Dictionary<Vector2, BuildingController> _activeRoads = new ();
        
        public RoadsService(TileManager tileManager)
        {
            _tileManager = tileManager;
        }
        
        public void Initialize()
        {
            
        }

        public void RegisterRoad(BuildingController selectedBuilding, ITile tile)
        {
            _activeRoads.Add(tile.Coords, selectedBuilding);
            
            InitializeRoads(selectedBuilding, tile);
        }
     
        private void InitializeRoads(BuildingController selectedBuilding, ITile tile)
        {
            var road = selectedBuilding.View as RoadView;
            if (road == null)
            {
                return;
            }

            var x = tile.Coords.x;
            var y = tile.Coords.y;

            for (var i = x - 1; i <= x + 1; i++)
            {
                for (var j = y - 1; j <= y + 1; j++)
                {
                    if (i < 0 || j < 0)
                    {
                        continue;
                    }

                    var tileToCheckCoord = new Vector2(i, j);
                    var tileToCheck = _tileManager.GetTileByCoord(tileToCheckCoord);

                    if (tileToCheck?.BuildingController?.Data.Type == BuildingType.RoadBuilding)
                    {
                        UpdateRoadVariant(i, j);
                    }
                }
            }
        }
            
        private void UpdateRoadVariant(int x, int y)
        {
            var coord = new Vector2(x, y);
            if (!_activeRoads.TryGetValue(coord, out var buildingController))
            {
                return;
            }

            var roadView = buildingController.View as RoadView;
            if (roadView == null)
            {
                return;
            }

            var hasNorth = _activeRoads.ContainsKey(new Vector2(x, y + 1));
            var hasSouth = _activeRoads.ContainsKey(new Vector2(x, y - 1));
            var hasEast = _activeRoads.ContainsKey(new Vector2(x + 1, y));
            var hasWest = _activeRoads.ContainsKey(new Vector2(x - 1, y));

            var (roadType, rotation) = DetermineRoadTypeAndRotation(hasNorth, hasSouth, hasEast, hasWest);
    
            roadView.SetRoadType(roadType, rotation);
        }
        
        private (RoadType type, float rotation) DetermineRoadTypeAndRotation(bool north, bool south, bool east, bool west)
        {
            var connectionCount = (north ? 1 : 0) + (south ? 1 : 0) + (east ? 1 : 0) + (west ? 1 : 0);
            
            switch (connectionCount)
            {
                case 0:
                    return (RoadType.Single, 0f); 
            
                case 1:
                    if (north) return (RoadType.DeadEnd, 0f);
                    if (east) return (RoadType.DeadEnd, 90f);
                    if (south) return (RoadType.DeadEnd, 180f);
                    if (west) return (RoadType.DeadEnd, 270f);
                    break;
            
                case 2:
                    if (north && south) return (RoadType.Straight, 0f); 
                    if (east && west) return (RoadType.Straight, 90f); 
            
                    if (north && east) return (RoadType.Corner, 0f);   // ┗ shape
                    if (east && south) return (RoadType.Corner, 90f);  // ┏ shape
                    if (south && west) return (RoadType.Corner, 180f); // ┓ shape
                    if (west && north) return (RoadType.Corner, 270f); // ┛ shape
                    break;
            
                case 3:
                    if (!north) return (RoadType.TJunction, 180f); 
                    if (!east) return (RoadType.TJunction, 270f);  
                    if (!south) return (RoadType.TJunction, 0f);   
                    if (!west) return (RoadType.TJunction, 90f);   
                    break;
            
                case 4:
                    return (RoadType.Crossroads, 0f); 
            }

            return (RoadType.Single, 0f);
        }
        
        public enum RoadType
        {
            Single,
            DeadEnd,
            Straight,
            Corner,
            TJunction,
            Crossroads
        }
            
    }
}