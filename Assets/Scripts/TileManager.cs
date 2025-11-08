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

    public void SetTileState(ITile tile, IMapContent obj)
    {
        var contentType = obj.MapContentType;
        var state = GetTileDataFromMapContent(contentType);
        tile.SetState(state);
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