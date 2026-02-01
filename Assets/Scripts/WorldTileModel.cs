using SurviveProject;
using UnityEngine;

public class WorldTileModel
{
    public readonly int Id;
    public readonly Vector2Int Coords;

    public TileState TileState;
    
    public IMapContent MapContent;
    public BuildingController BuildingController;
    
    public WorldTileModel(int id, Vector2Int coords)
    {
        Id = id;
        Coords = coords;
    }
}