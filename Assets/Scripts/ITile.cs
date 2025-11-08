using SurviveProject;
using UnityEngine;

public interface ITile
{
    public Transform MapContentHolder { get; }
    public Transform BuildingHolder { get; }

    public TileState TileState { get; }
    public void SetState(TileState state);
    void SetContent(IMapContent mapContent);
    void SetBuilding(BuildingController buildingController);
}

public enum TileState
{
    Free,
    Occupied
}