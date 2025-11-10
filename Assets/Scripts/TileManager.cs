using System.Collections.Generic;
using SurviveProject;

public class TileManager
{
    public Dictionary<int, ITile> ActiveTiles { get; } = new Dictionary<int, ITile>();
    
    public TileManager()
    {
        
    }
    
    public void AddTile(int id, ITile tile)
    {
        ActiveTiles.Add(id, tile);
    }

    private TileState GetTileDataFromMapContent(MapContentType objectDataType)
    {
        var tileState = objectDataType switch
        {
            MapContentType.Resource => TileState.Occupied,
            MapContentType.Obstacle => TileState.Occupied,
            _ => TileState.Free
        };

        return tileState;
    }

    public ITile GetTileByView(WorldTileView worldTileView)
    {
        var id = worldTileView.Id;
        return ActiveTiles[id];
    }
}