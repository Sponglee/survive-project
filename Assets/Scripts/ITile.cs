using SurviveProject;
using UnityEngine;

public interface ITile
{
    Transform MapContentHolder { get; }
    Transform BuildingHolder { get; }
    TileState TileState { get; }
    bool IsEmpty { get; }
    bool HasContent { get; }
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