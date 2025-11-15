    public interface IBuildingStrategy
    {
        public void Initialize(BuildingData targetBuildingData);
        public void DeselectBuilding();
        void TileHoverHandler(WorldTileView tile);
        void TileClickHandler(WorldTileView obj);
        void CancelBuildHandler();
        void MultiBuildModifierChangedHandler(bool toggle);
        void Dispose();
    }
