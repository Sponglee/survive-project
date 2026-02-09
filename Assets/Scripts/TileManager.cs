using System.Collections.Generic;
using SurviveProject;
using UnityEngine;

public class TileManager
{
    public Dictionary<Vector2, ITile> ActiveTiles { get; } = new Dictionary<Vector2, ITile>();
    public Dictionary<Vector2, WorldTileView> TileViews { get; } = new Dictionary<Vector2, WorldTileView>();
    
    public TileManager()
    {
        
    }
    
    public void AddTile(Vector2 coords, ITile tile, WorldTileView view)
    {
        ActiveTiles.Add(coords, tile);
        TileViews.Add(coords, view);
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
        var coords = worldTileView.TileCoords;
        return ActiveTiles[coords];
    }

    public ITile GetTileByCoord(Vector2 tileToCheckCoord)
    {
        ActiveTiles.TryGetValue(tileToCheckCoord, out var tile);
        
        return tile;
    }
    
    public WorldTileView GetViewByCoord(Vector2 tileToCheckCoord)
    {
        TileViews.TryGetValue(tileToCheckCoord, out var view);
        
        return view;
    }
}