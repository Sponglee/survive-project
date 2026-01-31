    using System;

    public interface IBuildingStrategy
    {
        public void Initialize(BuildingData targetBuildingData);
        void BuildingTileCheck(WorldTileView view);
        public void BuildingTileClick(WorldTileView obj);
        void CancelBuild();
        void ChangeMultiBuildModifier(bool toggle);
        void RotateBuilding(float angle);
        event Action<WorldTileView> OnCompletedBuild;
        bool TryCompleteBuilding(WorldTileView worldTileView);
    }
