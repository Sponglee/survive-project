using SurviveProject;
using UnityEngine;

public interface ITile
{
    Transform MapContentHolder { get; }
    Transform BuildingHolder { get; }
    BuildingController BuildingController { get; }
    TileState TileState { get; }
    bool IsEmpty { get; }
    bool HasContent { get; }
    public Vector2 Coords { get; }

    public MapContentType MapContentType { get; }
    void SetState(TileState state);
    void SetContent(IMapContent mapContent);
    void SetBuilding(BuildingController buildingController);
}

public enum TileState
{
    Free,
    Occupied
}