    public interface IBuildingStrategy
    {
        public void Initialize(BuildingData targetBuildingData);
        void BuildingTileCheck(WorldTileView view);
        public bool TryCompleteBuilding(WorldTileView obj);
        void CancelBuild();
        void ChangeMultiBuildModifier(bool toggle);
        void RotateBuilding(float angle);
    }
