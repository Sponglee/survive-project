using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class RoadsService : IInitializable
    {
        private readonly TileManager _tileManager;
        
        public RoadsService(TileManager tileManager)
        {
            _tileManager = tileManager;
        }
        
        public void Initialize()
        {
            
        }
     
        public void InitializeRoad(BuildingController selectedBuilding, ITile tile)
        {
            var road = selectedBuilding.View as RoadView;

            if (road == null)
            {
                return;
            }

            var neighborcount = 0;
            
            var x = tile.Coords.x;
            var y = tile.Coords.y;

            for (var i = x-1; i < x+1; i++)
            {
                for (var j = y-1; j < y+1; j++)
                {
                    if (i < 0 || j < 0)
                    {
                        continue;
                    }
                    
                    var tileToCheckCoord = new Vector2(i, j);
                
                    var tileToCheck = _tileManager.GetTileByCoord(tileToCheckCoord);

                    if (tileToCheck.BuildingController?.Data.Type == BuildingType.RoadBuilding)
                    {
                        neighborcount++;
                    }
                }
            }
            
            road.EnableRoadVariantById(neighborcount);
        }
    }
}