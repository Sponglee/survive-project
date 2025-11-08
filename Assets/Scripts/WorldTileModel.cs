using SurviveProject;
using UnityEngine;

public class WorldTileModel
{
    public readonly int Id;
    public readonly Vector2 Coords;

    public TileState TileState;
    
    public IMapContent MapContent;
    public BuildingController BuildingController;
    
    public WorldTileModel(int id, Vector2 coords)
    {
        Id = id;
        Coords = coords;
    }
}