using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SurviveProject
{
    public class RoadsService : IInitializable
    {
        private readonly TileManager _tileManager;
        private readonly RoadsPreset _roadsPreset;

        // Store all road tiles and their connections
        private Dictionary<Vector2Int, RoadTile> _roadTiles = new Dictionary<Vector2Int, RoadTile>();
        
        public RoadsService(TileManager tileManager, RoadsPreset roadsPreset)
        {
            _tileManager = tileManager;
            _roadsPreset = roadsPreset;
        }
        
        public void Initialize()
        {
            
        }

        public void RegisterRoadTile(BuildingController roadController, ITile tile)
        {
            var coords = tile.Coords;
            
            if (_roadTiles.ContainsKey(coords))
            {
                return; // Already exists
            }
            
            var roadTile = new RoadTile(tile, roadController);
            _roadTiles[coords] = roadTile;
        }
        
        public bool HasRoadAt(Vector2Int coords)
        {
            return _roadTiles.ContainsKey(coords);
        }
        
        public RoadTile GetRoadTile(Vector2Int coords)
        {
            _roadTiles.TryGetValue(coords, out var roadTile);
            return roadTile;
        }
        
        public void CreateConnection(Vector2Int coordsA, Vector2Int coordsB, GameObject roadElementPrefab)
        {
            if (!_roadTiles.TryGetValue(coordsA, out var tileA) || 
                !_roadTiles.TryGetValue(coordsB, out var tileB))
            {
                return;
            }
            
            // Check if connection already exists
            if (tileA.IsConnectedTo(coordsB))
            {
                return;
            }
            
            var connectionPointA = tileA.GetConnectionPoint();
            var connectionPointB = tileB.GetConnectionPoint();
            
            if (connectionPointA == null || connectionPointB == null)
            {
                return;
            }
            
            // Spawn road element
            var roadElementObj = Object.Instantiate(roadElementPrefab);
            var roadElement = roadElementObj.GetComponent<RoadElementView>();
            
            if (roadElement == null)
            {
                Object.Destroy(roadElementObj);
                return;
            }
            
            // Parent and position
            roadElementObj.transform.SetParent(connectionPointA);
            roadElementObj.transform.localPosition = Vector3.zero;
            roadElementObj.transform.localRotation = Quaternion.identity;
            roadElementObj.transform.localScale = Vector3.one;
            
            // Set line renderer
            roadElement.SetConnectionPoints(connectionPointA.position, connectionPointB.position);
            
            // Store bidirectional connection
            tileA.AddConnection(coordsB, roadElement);
            tileB.AddConnection(coordsA, roadElement);
        }
        
        public void RemoveRoadTile(Vector2Int coords)
        {
            if (!_roadTiles.TryGetValue(coords, out var roadTile))
            {
                return;
            }
            
            // Remove all connections
            var connectedCoords = new List<Vector2Int>(roadTile.Connections.Keys);
            foreach (var connectedCoord in connectedCoords)
            {
                roadTile.RemoveConnection(connectedCoord);
                
                if (_roadTiles.TryGetValue(connectedCoord, out var connectedTile))
                {
                    connectedTile.RemoveConnection(coords);
                }
            }
            
            // Dispose
            roadTile.RoadController.Dispose();
            _roadTiles.Remove(coords);
            
            // Reset tile
            roadTile.Tile.SetState(TileState.Free);
            roadTile.Tile.SetBuilding(null);
        }
    }
}